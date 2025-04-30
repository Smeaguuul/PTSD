using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;
using DataAccess.Context;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class MatchesService : IMatchesService
    {
        private readonly IRepository<Match> repository;

        public MatchesService()
        {
            repository = new Repository<Match>(new AppDBContext());
        }

        public async Task<IEnumerable<Match>> OngoingMatches()
        {
            var matches = await repository.GetAllAsync(
                predicate: m => m.Status == Status.Ongoing,
                include: query => query
                .Include(m => m.Score)
                .ThenInclude(s => s.Sets)
                .ThenInclude(s => s.Games)
                );
            return matches;
        }
    }
}
