using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;
using HotelListing.Repositories;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{id:int}", Name = "GetCountry")]
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

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attemp in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = _mapper.Map<Country>(countryDTO);
                await _unitOfWOrk.CountryGenericRepository.Insert(country);
                await _unitOfWOrk.Save();
                return CreatedAtRoute("GetCountry", new { Id = country.Id }, country);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, $"Somthing were wrong with {nameof(CreateCountry)}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid PUT attemp in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = await _unitOfWOrk.CountryGenericRepository.Get(x => x.Id == id);
                if (country == null)
                {
                    _logger.LogError($"Invalid PUT attemp in {nameof(UpdateCountry)}");
                    return BadRequest(ModelState);
                }

                _mapper.Map(countryDTO, country);
                _unitOfWOrk.CountryGenericRepository.Update(country);
                await _unitOfWOrk.Save();
                return NoContent();
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, $"Somthing were wrong with {nameof(UpdateCountry)}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attemp in {nameof(DeleteCountry)}");
                return BadRequest("Somthing data is invalid");
            }

            try
            {
                var country = await _unitOfWOrk.CountryGenericRepository.Get(x => x.Id == id);
                if (country == null)
                {
                    _logger.LogError($"Invalid DELETE attemp in {nameof(DeleteCountry)}");
                    return BadRequest("Somthing data is invalid");
                }

                await _unitOfWOrk.CountryGenericRepository.Delete(id);
                await _unitOfWOrk.Save();
                return NoContent();
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, $"Somthing were wrong with {nameof(DeleteCountry)}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
