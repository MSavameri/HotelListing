using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;
using HotelListing.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork,
            ILogger<HotelController> logger,
            IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork.HotelGenericRepository.GetAll();
                var result = _mapper.Map<IList<HotelDTO>>(hotels);
                return Ok(result);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, $"Somting were wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "internal server error");
            }
        }


        [HttpGet("{id:int}", Name = "GetHotel")]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork.HotelGenericRepository.Get(x => x.Id == id, new List<string> { "Country" });
                var result = _mapper.Map<HotelDTO>(hotel);
                return Ok(result);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, $"Somthing were wrong with {nameof(GetHotel)}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attemp in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDTO);
                await _unitOfWork.HotelGenericRepository.Insert(hotel);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetHotel", new { Id = hotel.Id }, hotel);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, $"Somthing were wrong with {nameof(CreateHotel)}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid PUT attemp in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = await _unitOfWork.HotelGenericRepository.Get(x => x.Id == id);
                if (hotel == null)
                {
                    _logger.LogError($"Invalid PUT attemp in {nameof(UpdateHotel)}");
                    return BadRequest(ModelState);
                }

                _mapper.Map(hotelDTO, hotel);
                _unitOfWork.HotelGenericRepository.Update(hotel);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, $"Somthing were wrong with {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attemp in {nameof(DeleteHotel)}");
                return BadRequest("Somthing data is invalid");
            }

            try
            {
                var hotel = await _unitOfWork.HotelGenericRepository.Get(x => x.Id == id);
                if (hotel == null)
                {
                    _logger.LogError($"Invalid DELETE attemp in {nameof(DeleteHotel)}");
                    return BadRequest("Somthing data is invalid");
                }

                await _unitOfWork.HotelGenericRepository.Delete(id);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, $"Somthing were wrong with {nameof(DeleteHotel)}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
