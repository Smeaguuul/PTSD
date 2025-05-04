using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ClubsService
    {
        private readonly IRepository<Club> Repository;
        IMapper Mapper;

        public ClubsService(IMapper mapper, IRepository<Club> repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        public async Task<IEnumerable<DTO.Club>> GetAll()
        {
            var clubs = await Repository.GetAllAsync(
                include: query => query
                .Include(c => c.Teams).ThenInclude(t => t.Players)
                );
            return Mapper.Map<List<DTO.Club>>(clubs);
        }
    }
}
