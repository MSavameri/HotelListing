using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
              new Hotel { Id = 1, Name = "Hotel Enghelab", Adress = "Tehran", Rating = 4.5, CountryId = 1 },
              new Hotel { Id = 2, Name = "Hotel Pars", Adress = "Shiraz", Rating = 4, CountryId = 1 },
              new Hotel { Id = 3, Name = "Hotel AlKhalifeh", Adress = "Dubai", Rating = 5, CountryId = 2 }
              );
        }
    }
}
