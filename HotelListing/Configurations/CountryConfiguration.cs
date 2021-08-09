﻿using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
               new Country { Id = 1, Name = "Iran", ShortName = "IR" },
               new Country { Id = 2, Name = "United Emirate", ShortName = "UAE" }
               );
        }
    }
}
