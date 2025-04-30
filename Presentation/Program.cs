/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();*/

using AutoMapper;
using Business.Mappers;
using DataAccess.Context;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MapperProfile>();
});

var mapper = config.CreateMapper();


var playerDto = new DTO.Player
{
    Id = 1,
    Name = "Test Player",
    // other properties...
};

// Map it to the domain model
var playerDomain = mapper.Map<Player>(playerDto);

Console.WriteLine($"Mapped Player: {playerDomain.Name}, Id: {playerDomain.Id}");


Club club = new Club("FC Barcelona", "BAR", "Barcelona");
Team team = new Team(1, "FC Barcelona", new List<Player> { new Player(1, "Lionel Messi"), new Player(2, "Xavi Hernandez") });
Game game = new Game(1, true, 1, new List<bool> { true, false, true });
Set set = new Set(1, true, new List<Game> { game });

//IRepository<Club> clubRepository = new Repository<Club>(new AppDBContext());
//clubRepository.AddAsync(club).Wait();




