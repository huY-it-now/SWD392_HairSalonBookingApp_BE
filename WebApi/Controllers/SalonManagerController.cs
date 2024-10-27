using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using System.Threading.Tasks;

namespace SWD392_HairSalonBookingApp_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalonManagerController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public SalonManagerController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetBookings()
        {
            try
            {
                var bookings = await _bookingService.ShowAllCheckedBooking();
                return Ok(bookings);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving bookings.", details = ex.Message });
            }
        }
    }
}

