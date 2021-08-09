using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;
using HotelListing.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWOrk;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWOrk,
            ILogger<CountryController> logger,
            IMapper mapper)
        {
            _unitOfWOrk = unitOfWOrk;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWOrk.CountryGenericRepository.GetAll();
                var result = _mapper.Map<IList<CountryDTO>>(countries);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somting went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "internal server error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWOrk.CountryGenericRepository.Get(x => x.Id == id,
                    new List<string> { "Hotels" });
                var result = _mapper.Map<CountryDTO>(country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somting went wrog in the {nameof(GetCountry)}");
                return StatusCode(500, "internal server error");
            }
        }
    }
}
