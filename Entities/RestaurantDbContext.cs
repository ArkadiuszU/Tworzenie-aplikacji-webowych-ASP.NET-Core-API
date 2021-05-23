using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Entities
{

    public class RestaurantDbContext : DbContext
    {
// private string _connectionString = "Server = (localdb)\\MSSQLLocalDB;Database=RestaurantDb;Trusted_Connection=True;";
        private string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = master; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>().Property(r => r.Name).IsRequired().HasMaxLength(25);

            modelBuilder.Entity<Dish>().Property(r => r.Name).IsRequired();

            modelBuilder.Entity<Adress>().Property(r => r.City).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Adress>().Property(r => r.Street).IsRequired().HasMaxLength(50);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {

            optionBuilder.UseSqlServer(_connectionString);

        }


    }
}
