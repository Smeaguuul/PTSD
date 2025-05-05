using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Models;
using DataAccess.Models.Giveaways;
using DTO.Giveaway;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Business.Mappers
{
    public class MapperProfile : Profile
    {

        /// <summary>
        /// Mapper profile for mapping between DataAccess models and DTOs.
        /// </summary>
        public MapperProfile()
        {
            CreateMap<Club, DTO.Club>();
            CreateMap<DTO.Club, Club>();

            CreateMap<Game, DTO.Game>();
            CreateMap<DTO.Game, Game>();

            CreateMap<Match, DTO.Match>();
            CreateMap<DTO.Match, Match>();

            CreateMap<Player, DTO.Player>();
            CreateMap<DTO.Player, Player>();

            CreateMap<Score, DTO.Score>();
            CreateMap<DTO.Score, Score>();

            CreateMap<Set, DTO.Set>();
            CreateMap<DTO.Set, Set>();

            CreateMap<Status, DTO.Status>();
            CreateMap<DTO.Status, Status>();

            CreateMap<Team, DTO.Team>();
            CreateMap<DTO.Team, Team>();

            CreateMap<ContestantDto, Contestant>();
            CreateMap<Contestant, ContestantDto>();

            CreateMap<Giveaway, GiveawayDto>();
            CreateMap<GiveawayDto, Giveaway>();

            CreateMap<DataAccess.Models.Giveaways.GiveawayStatus, DTO.Giveaway.GiveawayStatus>();
            CreateMap<DTO.Giveaway.GiveawayStatus, DataAccess.Models.Giveaways.GiveawayStatus>();
        }

      

    }

}
