using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions option) : base(option)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "Iran", ShortName = "IR" },
                new Country { Id = 2, Name = "United Emirate", ShortName = "UAE" }
                );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel { Id = 1, Name = "Hotel Enghelab", Adress = "Tehran", Rating = 4.5, CountryId = 1 },
                new Hotel { Id = 2, Name = "Hotel Pars", Adress = "Shiraz", Rating = 4, CountryId = 1 },
                new Hotel { Id = 3, Name = "Hotel AlKhalifeh", Adress = "Dubai", Rating = 5, CountryId = 2 }
                );
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }


    }
}
