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
        private readonly ISalonService _salonService;
        private readonly ISalonMemberService _salonMemberService;
        private readonly IComboService _comboService;
        private readonly IServiceService _serviceService;

        public BookingController(IBookingService bookingService, IMapper mapper, IServiceService serviceService, IComboService comboService, IUserService userService, ISalonMemberService salonMemberService, ISalonService salonService)
        {
            _salonService = salonService;
            _salonMemberService = salonMemberService;
            _comboService = comboService;
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
        public async Task<IActionResult> AddBooking([FromForm] Guid CustomerId, Guid SalonId, Guid SalonMemberId, DateTime cuttingDate, Guid ServiceId, string ComboServiceId)
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

            var serviceObj = await _serviceService.GetServiceById(ServiceId);

            if (serviceObj.Data == null)
            {
                return BadRequest("Service not found");
            }

            var service = (Service)serviceObj.Data;

            if (service != null)
            {
                booking.Service = service;
            }

            foreach (var id in ExtractValidIds(ComboServiceId))
            {
                var comboService = await _comboService.GetComboServiceById(new Guid(id));

                if (comboService.Data != null)
                {
                    var combo = (ComboService)comboService.Data;

                    //booking.Service.ServiceComboServices.Add(combo);

                    TotalAmount += combo.Price;
                }
            }

            var Salon = await _salonService.GetSalonById(SalonId);

            if (Salon != null)
            {
                //foreach (var item in booking.Service.ComboServices)
                //{
                //    item.SalonId = SalonId;
                //    item.Salon = Salon;
                //}                
            }

            var User = await _userService.GetUserById(CustomerId);
            var user = User.Data;

            if (user != null)
            {
                booking.User = (User)user;
                booking.UserId = CustomerId;
            }

            booking.TotalMoney = TotalAmount;

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
