using Application.Interfaces;
using Application.Services;
using Application.Validations.Account;
using AutoMapper;
using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Account;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace WebApi.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [HttpGet("ShowUncheckedBooking")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> PrintAllUncheckedBooking()
        {
            var bookingList = _bookingService.ShowAllUncheckedBooking();

            return Ok(bookingList);
        }

        [HttpPost("CheckBooking")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> CheckBooking(Guid bookingId, bool Check) // true = ok, false = fake (delete)
        {
            var result = await _bookingService.CheckBooking(bookingId, Check);

            return Ok(result);
        }

        [HttpPost("AddBooking")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> AddBooking([FromForm] Guid CustomerId, Guid SalonId, Guid SalonMemberId, DateTime cuttingDate, string ServiceId, string ComboServiceId)
        { 


            return Ok();
        }
    }
}
