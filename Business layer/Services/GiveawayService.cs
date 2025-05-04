using AutoMapper;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.Giveaways;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class GiveawayService
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



        public async Task deleteGiveaway(int giveawayID)
        {
            
        }




        public async Task<bool> AddContestantToGiveawayAsync(int giveawayId, string email)
        {
            // Step 1: Check if the giveaway exists and load contestants
            var giveaway = await GiveawayRepository.FirstOrDefaultAsync(
                g => g.Id == giveawayId,
                include: query => query.Include(g => g.GiveawayContestants));

            if (giveaway == null)
                throw new ArgumentException("Giveaway not found.");

            // Step 2: Check if the giveaway is currently active
            var now = DateTime.Now;
            if (now < giveaway.StartDate)
                throw new InvalidOperationException("The giveaway has not started yet.");
            if (now > giveaway.EndDate)
                throw new InvalidOperationException("The giveaway has already ended.");

            // Step 3: Check if the contestant is already linked to this giveaway
            var alreadyInGiveaway = await GiveawayContestantRepository.FirstOrDefaultAsync(
                gc => gc.GiveawayId == giveawayId && gc.contestant.Email == email,
                include: query => query.Include(gc => gc.contestant));

            if (alreadyInGiveaway != null)
                return false; // Contestant already in giveaway

            // Step 4: Check if the contestant exists
            var contestant = await ContestantRepository.FirstOrDefaultAsync(c => c.Email == email);

            // Step 5: If not, create it
            if (contestant == null)
            {
                contestant = new Contestant(email);
                await ContestantRepository.AddAsync(contestant);
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



        public List<string> GetContestants()
        {
            //return list of contestants
            throw new NotImplementedException();
        }

        public List<Contestant> PickWinner(int amountOfWinners)
        {
            //pick winner from list of contestants
            throw new NotImplementedException();
        }

        public Contestant PickWinner()
        {
            //pick winner from list of contestants
            throw new NotImplementedException();
        }



    }
}
