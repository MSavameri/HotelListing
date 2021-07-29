using AutoMapper;
using HotelListing.Models;
using HotelListing.Repositories;
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


        [HttpGet]
        [Route("{id:int}")]
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
    }
}
