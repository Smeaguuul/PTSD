﻿using AutoMapper;
using Business.Interfaces;
using Business.Mappers;
using Business.Services;
using DataAccess.Context;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Models.Giveaways;
using DataAccess.Repositories;
using DataAccess.SeedData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MapperProfile>();
});

var mapper = config.CreateMapper();

//Dependency Injection
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddDbContext<AppDBContext>();
builder.Services.AddScoped<IRepository<Match>, Repository<Match>>();
builder.Services.AddScoped<IRepository<Club>, Repository<Club>>();
builder.Services.AddScoped<IRepository<Team>, Repository<Team>>();
builder.Services.AddScoped<IRepository<Giveaway>, Repository<Giveaway>>();
builder.Services.AddScoped<IRepository<Contestant>, Repository<Contestant>>();
builder.Services.AddScoped<IRepository<GiveawayContestant>, Repository<GiveawayContestant>>();
builder.Services.AddScoped<IRepository<AdminUser>, Repository<AdminUser>>();
builder.Services.AddScoped<IMatchesService, MatchesService>();
builder.Services.AddScoped<IClubsService, ClubsService>();
builder.Services.AddScoped<IGiveawayService, GiveawayService>();
builder.Services.AddScoped<AdminUserService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<Seeder>();



builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Admin/Login";
    });

builder.Services.AddAuthorization();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seedData = scope.ServiceProvider.GetRequiredService<DataAccess.SeedData.Seeder>();
    await seedData.SeedGenerator();  // Call the method to seed data
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();






//var playerDto = new DTO.Player
//{
//    Id = 1,
//    Name = "Test Player",
//    // other properties...
//};

//// Map it to the domain model
//var playerDomain = mapper.Map<Player>(playerDto);

//Console.WriteLine($"Mapped Player: {playerDomain.Name}, Id: {playerDomain.Id}");



