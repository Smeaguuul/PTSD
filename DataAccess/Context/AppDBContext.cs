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
    internal class AppDBContext : DbContext
    {
        public AppDBContext()
        {
        }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>().HasKey(c => c.Abbriviation);
            modelBuilder.Entity<Game>().HasKey(g => g.Id);
            modelBuilder.Entity<Match>().HasKey(m => m.Id);
            modelBuilder.Entity<Player>().HasKey(p => p.Id);
            modelBuilder.Entity<Score>().HasKey(s => s.Id);
            modelBuilder.Entity<Set>().HasKey(s => s.Id);
            modelBuilder.Entity<Team>().HasKey(t => t.Id);

            // Configure your entities here
            // modelBuilder.Entity<YourEntity>().ToTable("YourTableName");
        }
        protected override void
 OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "";

            using (StreamReader r = new StreamReader("Connection.json"))
            {
                connectionString = JsonDocument.Parse(r.ReadToEnd()).RootElement.GetProperty("ConnectionString").GetString() ?? "";

            }

            optionsBuilder.UseSqlServer(connectionString);
 }
    }
}
