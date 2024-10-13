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
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ISalonMemberService _salonMemberService;
        private readonly IComboServiceService _comboServiceService;
        private readonly IServiceService _serviceService;

        public BookingController(IBookingService bookingService, IMapper mapper, IServiceService serviceService, IComboServiceService comboServiceService, IUserService userService, ISalonMemberService salonMemberService)
        {
            _salonMemberService = salonMemberService;
            _comboServiceService = comboServiceService;
            _serviceService = serviceService;
            _bookingService = bookingService;
            _userService = userService;
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
            Decimal TotalAmount = 0;
            Booking booking = new Booking();

            booking.Checked = false;
            booking.BookingDate = DateTime.Now;
            booking.SalonMemberId = SalonMemberId;
            booking.UserId = CustomerId;

            var salonMember = await _salonMemberService.GetSalonMemberById(SalonMemberId);

            if (salonMember != null)
            {
                booking.SalonMember = salonMember;

                booking.SalonMemberId = SalonMemberId;
            }

            var service = await _serviceService.GetServiceById(ServiceId);

            if (service != null)
            {
                booking.Service = service;

                TotalAmount += service.Money;
            }

            foreach (var id in ExtractValidIds(ComboServiceId))
            {
                var comboService = await _comboServiceService.GetComboServiceByIdAsync(new Guid(id));

                if (comboService != null)
                {
                    booking.ComboServices.Add(comboService);

                    TotalAmount += comboService.Price;
                }
            }

            if (!await _bookingService.CreateBooking(booking))
            {
                return BadRequest("Create booking fail");
            }

            return Ok(booking);
        }

        List<string> ExtractValidIds(string input)
        {
            List<string> result = new List<string>();
            int start = 0;

            while (start < input.Length)
            {
                bool found = false;
                
                // Try different lengths starting from the current position
                for (int length = 36; length <= input.Length - start; length++)
                {
                    string potentialGuid = input.Substring(start, length);

                    // Validate if the substring is a valid GUID
                    if (Guid.TryParse(potentialGuid, out _))
                    {
                        result.Add(potentialGuid);  // Add the valid GUID to the result list
                        start += length;            // Move start index to the end of the found GUID
                        found = true;
                        break;                      // Break out to find the next GUID
                    }
                }

                // If no valid GUID was found, increment the start position
                if (!found)
                {
                    start++;
                }
            }

            return result;
        }
    }
}
