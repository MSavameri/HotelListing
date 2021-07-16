using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configurations
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer()
        {
            CreateMap<Country, CountryDTO>();
            CreateMap<Country, CreateCountryDTO>();
            CreateMap<Hotel, HotelDTO>();
            CreateMap<Hotel, CreateHotelDTO>();
        }   
    }
}
