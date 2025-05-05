using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Mappers;
using Business.Services;
using DataAccess.Context;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Models.Giveaways;
using DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Assist;
using Reqnroll.Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    public class SetupTestDependencies
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();

            // TODO: add your test dependencies here
            // Add services to the container.
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });

            var mapper = config.CreateMapper();

            //Dependency Injection
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddDbContext<AppDBContext>();
            services.AddScoped<IRepository<Match>, Repository<Match>>();
            services.AddScoped<IRepository<Club>, Repository<Club>>();
            services.AddScoped<IRepository<Team>, Repository<Team>>();
            services.AddScoped<IRepository<Giveaway>, Repository<Giveaway>>();
            services.AddScoped<IRepository<Contestant>, Repository<Contestant>>();
            services.AddScoped<IRepository<GiveawayContestant>, Repository<GiveawayContestant>>();
            services.AddScoped<MatchesService>();
            services.AddScoped<ClubsService>();
            services.AddScoped<GiveawayService>();

            return services;
        }
    }
}
