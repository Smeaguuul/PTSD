using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    //Run these commands at the project root: (Only the second if the migrations are already there, and the database just needs initlization)
    // dotnet ef migrations add InitialCreate --output-dir ./Context/Migrations
    // dotnet ef database update
    public class AppDBContext : DbContext
    {
        public AppDBContext()
        {
        }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>().HasKey(c => c.Abbreviation);
            modelBuilder.Entity<Game>().HasKey(g => g.Id);
            modelBuilder.Entity<Match>().HasKey(m => m.Id);
            modelBuilder.Entity<Player>().HasKey(p => p.Id);
            modelBuilder.Entity<Score>().HasKey(s => s.Id);
            modelBuilder.Entity<Set>().HasKey(s => s.Id);
            modelBuilder.Entity<Team>().HasKey(t => t.Id);

            // Configure your entities here
            // modelBuilder.Entity<YourEntity>().ToTable("YourTableName");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "";

            string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

            string filePath = Path.Combine(solutionDirectory, "DataAccess","Context", "Connection.json");
            StreamReader reader = new(filePath);
            connectionString = reader.ReadToEnd();
            connectionString = JsonDocument.Parse(connectionString).RootElement.GetProperty("Database_Connection_String").GetString() ?? "";

            //using (StreamReader r = new StreamReader("Connection.json"))
            //{
            //    connectionString = JsonDocument.Parse(r.ReadToEnd()).RootElement.GetProperty("Database_Connection_String").GetString() ?? "";
            //}

            optionsBuilder.UseSqlServer(connectionString);
 }
    }
}
