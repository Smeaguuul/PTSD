using AutoMapper;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.Giveaways;
using Microsoft.EntityFrameworkCore;
using DTO.Giveaway;
using Business.Interfaces;

namespace Business.Services
{
    public class GiveawayService : IGiveawayService
    {

        private readonly IRepository<Giveaway> GiveawayRepository;
        private readonly IRepository<Contestant> ContestantRepository;
        private readonly IRepository<GiveawayContestant> GiveawayContestantRepository;
        IMapper Mapper;

        public GiveawayService(IMapper mapper, IRepository<Giveaway> repository, IRepository<Contestant> repCont, IRepository<GiveawayContestant> gcrep)
        {
            Mapper = mapper;
            GiveawayRepository = repository;
            ContestantRepository = repCont;
            GiveawayContestantRepository = gcrep;
        }


        public async Task<Giveaway> CreateGiveawayAsync(CreateGiveawayDto giveawayDto)
        {
            return await CreateGiveawayAsync(giveawayDto.Name, giveawayDto.Description, giveawayDto.StartDate, giveawayDto.EndDate);
        }

        public async Task<Giveaway> CreateGiveawayAsync(string name, string description, DateTime start, DateTime end)
        {
            //check if start and end date are valid
            var validationResult = CheckDateValidity(start, end);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.ErrorMessage);
            }


            var giveaway = new Giveaway(name, description, start, end);

            await GiveawayRepository.AddAsync(giveaway);
            return giveaway;
        }

        private class ValidationResult
        {
            public bool IsValid { get; set; }
            public string? ErrorMessage { get; set; }

            public static ValidationResult Success() => new ValidationResult { IsValid = true };
            public static ValidationResult Fail(string message) => new ValidationResult { IsValid = false, ErrorMessage = message };
        }


        private static ValidationResult CheckDateValidity(DateTime start, DateTime end)
        {
            if (start > end)
                return ValidationResult.Fail("Start date cannot be after end date.");

            if (start < DateTime.Now)
                return ValidationResult.Fail("Start date cannot be in the past.");

            return ValidationResult.Success();
        }



        public async Task DeleteGiveaway(int giveawayID)
        {

            //check if giveaway exists
            var giveaway = await GiveawayRepository.FirstOrDefaultAsync(g => g.Id == giveawayID);
            if (giveaway == null)
                throw new ArgumentException("Giveaway not found.");

            //check if giveaway is active
            if (giveaway.Status == DataAccess.Models.Giveaways.GiveawayStatus.Ongoing)
                throw new InvalidOperationException("Giveaway is ongoing. Deletion must be confirmed.");



            //Get all previosusly linked contestants
            var links = await GiveawayContestantRepository.GetAllAsync(gc => gc.GiveawayId == giveawayID);


            //Remove the contestants
            foreach (var link in links)
            {
                await RemoveContestantFromGiveawayAsync(giveawayID, link.ContestantId);
            }


            //delete giveaway
            await GiveawayRepository.RemoveAsync(giveaway);

        }



        public async Task<bool> AddContestantToGiveawayAsync(int giveawayId, string email, string name)
        {
            // Step 1: Check if the giveaway exists and load contestants
            var giveaway = await GiveawayRepository.FirstOrDefaultAsync(
                g => g.Id == giveawayId,
                include: query => query.Include(g => g.GiveawayContestants));

            if (giveaway == null)
                throw new ArgumentException("Giveaway not found.");

            // Step 2: Check if the giveaway is currently active
            var now = DateTime.Now;
            if (now > giveaway.EndDate)
                throw new InvalidOperationException("The giveaway has already ended.");

            // Step 3: Check if the contestant is already linked to this giveaway
            var alreadyInGiveaway = await GiveawayContestantRepository.FirstOrDefaultAsync(
                gc => gc.GiveawayId == giveawayId && gc.contestant.Email == email,
                include: query => query.Include(gc => gc.contestant));

            if (alreadyInGiveaway != null)
                return false; // Contestant already entered

            // Step 4: Check if the contestant exists by email
            var contestant = await ContestantRepository.FirstOrDefaultAsync(c => c.Email == email);

            // Step 5: If not, create new contestant with both email and name
            if (contestant == null)
            {
                contestant = new Contestant
                {
                    Email = email,
                    Name = name
                };
                await ContestantRepository.AddAsync(contestant);
            }
            else if (string.IsNullOrWhiteSpace(contestant.Name) && !string.IsNullOrWhiteSpace(name))
            {
                contestant.Name = name;
                await ContestantRepository.UpdateAndSaveAsync(contestant);
            }

            // Step 6: Link contestant to giveaway
            var link = new GiveawayContestant(giveaway, contestant);
            await GiveawayContestantRepository.AddAsync(link);

            return true;
        }




        public async Task<bool> RemoveContestantFromGiveawayAsync(int giveawayId, int contestantId)
        {
            // Find the linking entity
            var link = await GiveawayContestantRepository.FirstOrDefaultAsync(gc =>
                gc.GiveawayId == giveawayId && gc.ContestantId == contestantId);

            if (link == null)
                return false;

            // Remove the link
            await GiveawayContestantRepository.RemoveAsync(link);

            // Check if the contestant is still linked to any giveaways
            var stillLinked = (await GiveawayContestantRepository.GetAllAsync(gc =>
                gc.ContestantId == contestantId)).Any();

            if (!stillLinked)
            {
                var orphan = await ContestantRepository.FirstOrDefaultAsync(c => c.Id == contestantId);
                if (orphan != null)
                    await ContestantRepository.RemoveAsync(orphan);
            }

            return true;
        }


        public async Task<IEnumerable<DTO.Giveaway.GiveawayDto>> GetGiveaways()
        {
            var giveaways = await GiveawayRepository.GetAllAsync(
                include: query => query.Include(g => g.GiveawayContestants).ThenInclude(gc => gc.contestant)
                );
            return Mapper.Map<IEnumerable<DTO.Giveaway.GiveawayDto>>(giveaways);
        }



        public async Task<IEnumerable<DTO.Giveaway.ContestantDto>> GetContestants(int giveawayId)
        {
            var giveawayContestants = await GiveawayContestantRepository.GetAllAsync();
            var contestantsInGiveaway = giveawayContestants
                .Where(gc => gc.GiveawayId == giveawayId)
                .Select(gc => gc.contestant);

            return Mapper.Map<IEnumerable<DTO.Giveaway.ContestantDto>>(contestantsInGiveaway);
        }

        public async Task<IEnumerable<DTO.Giveaway.ContestantDto>> PickWinner(int amountOfWinners, int giveawayId)
        {
            var contestants = (await GetContestants(giveawayId)).ToList();

            if (amountOfWinners <= 0 || amountOfWinners > contestants.Count)
                throw new ArgumentException("Invalid number of winners.");

            // Randomize the list
            var rng = new Random();
            var shuffled = contestants.OrderBy(_ => rng.Next()).Take(amountOfWinners);

            return shuffled;
        }

        public async Task<DTO.Giveaway.ContestantDto> PickWinner(int giveawayId)
        {
            return (await PickWinner(1, giveawayId)).FirstOrDefault();
        }


        public async Task SeedData()
        {
            var giveaway1 = new Giveaway
            {
                Name = "Spring Giveaway",
                Description = "Win cool spring prizes!",
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(30)
            };

            var giveaway2 = new Giveaway
            {
                Name = "Summer Giveaway",
                Description = "Win cool summer prizes!",
                StartDate = DateTime.Now.AddDays(31),
                EndDate = DateTime.Now.AddDays(45)
            };

            var contestant1 = new Contestant { Name = "Alice", Email = "alice@example.com" };
            var contestant2 = new Contestant { Name = "Bob", Email = "bob@example.com" };
            var contestant3 = new Contestant { Name = "Charlie", Email = "charlie@example.com" };

            var giveawayContestant1 = new GiveawayContestant { contestant = contestant1, giveaway = giveaway1 };
            var giveawayContestant2 = new GiveawayContestant { contestant = contestant2, giveaway = giveaway1 };
            var giveawayContestant3 = new GiveawayContestant { contestant = contestant3, giveaway = giveaway2 };

            giveaway1.GiveawayContestants.Add(giveawayContestant1);
            giveaway1.GiveawayContestants.Add(giveawayContestant2);
            giveaway2.GiveawayContestants.Add(giveawayContestant3);

            await GiveawayRepository.AddAsync(giveaway1);
            await GiveawayRepository.AddAsync(giveaway2);
        }

    }
}
