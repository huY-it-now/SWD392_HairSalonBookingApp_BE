using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Domain.Contracts.DTO.Booking;
using Domain.Contracts.Abstracts.Shared;
using Application.Services;

namespace SWD392_HairSalonBookingApp_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalonManagerController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ISalonMemberService _salonMemberService;
        private readonly ILogger<SalonManagerController> _logger;

        public SalonManagerController(IBookingService bookingService,
            ISalonMemberService salonMemberService,
            ILogger<SalonManagerController> logger)
        {
            _bookingService = bookingService;
            _salonMemberService = salonMemberService;
            _logger = logger;
        }

        [HttpGet("bookings/unchecked")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetUncheckBookings()
        {
            try
            {
                var bookings = await _bookingService.ShowAllUncheckedBooking();
                return Ok(bookings);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving unchecked bookings.");
                return StatusCode(500, new { message = "An error occurred while retrieving unchecked bookings.", details = ex.Message });
            }
        }

        [HttpGet("bookings/checked")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetCheckedBookings()
        {
            try
            {
                var bookings = await _bookingService.ShowAllCheckedBooking();
                return Ok(bookings);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving checked bookings.");
                return StatusCode(500, new { message = "An error occurred while retrieving checked bookings.", details = ex.Message });
            }
        }

        [HttpGet("all-Stylists")]
        [ProducesResponseType(200, Type = typeof(List<StylistDTO>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        [ProducesResponseType(500, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetAllStylists()
        {
            try
            {
                var stylists = await _salonMemberService.GetAllStylists();
                var stylistDTOList = stylists.Select(stylist => new StylistDTO
                {
                    Id = stylist.Id,
                    FullName = stylist.FullName,
                    Email = stylist.Email,
                    Job = "Stylist",
                    Rating = stylist.Rating.ToString(),
                    Status = stylist.Status
                }).ToList();

                return Ok(stylistDTOList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving stylists.");
                return StatusCode(500, new { message = "An error occurred while retrieving stylists.", details = ex.Message });
            }
        }

        [HttpGet("all-SalonStaff")]
        [ProducesResponseType(200, Type = typeof(List<SalonStaffDTO>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        [ProducesResponseType(500, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetAllSalonStaff()
        {
            try
            {
                var salonStaff = await _salonMemberService.GetAllSalonStaff();
                var salonStaffDTOList = salonStaff.Select(staff => new SalonStaffDTO
                {
                    Id = staff.Id,
                    FullName = staff.FullName,
                    Email = staff.Email,
                    Job = "Salon Staff",
                    Status = staff.Status
                }).ToList();

                return Ok(salonStaffDTOList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving salon staff.");
                return StatusCode(500, new { message = "An error occurred while retrieving salon staff.", details = ex.Message });
            }
        }
    }
}
