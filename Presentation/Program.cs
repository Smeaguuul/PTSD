using AutoMapper;
using Business.Mappers;
using Business.Services;
using DataAccess.Context;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Models.Giveaways;
using DataAccess.Repositories;

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
builder.Services.AddScoped<MatchesService>();
builder.Services.AddScoped<ClubsService>();
builder.Services.AddScoped<GiveawayService>();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Admin/Login";
    });

builder.Services.AddAuthorization();



var app = builder.Build();

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



