using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Mappers;
using DataAccess.Context;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class MatchesService : IMatchesService
    {
        private readonly IRepository<Match> Repository;
        IMapper Mapper;

        public MatchesService(IMapper mapper, IRepository<Match> repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        public async Task<IEnumerable<DTO.Match>> OngoingMatches()
        {
            var matches = await Repository.GetAllAsync(
                include: query => query
                .Include(m => m.Score)
                .ThenInclude(s => s.Sets)
                .ThenInclude(s => s.Games)
                );
            return Mapper.Map<List<DTO.Match>>(matches);
        }
    }
}
