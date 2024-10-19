using Application.Interfaces;
using Application.Services;
using Application.Validations.Account;
using AutoMapper;
using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Account;
using Domain.Contracts.DTO.Booking;
using Domain.Contracts.DTO.Salon;
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
        public async Task<Result<object>> PrintAllUncheckedBooking()
        {
            var bookingList = await _bookingService.ShowAllUncheckedBooking();

            var result = new Result<object>
            {
                Error = 0,
                Message = "Checked",
                Data = bookingList
            };

            return result;
        }

        [HttpPost("CheckBooking")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<Result<object>> CheckBooking(Guid bookingId, bool Check) // true = ok, false = fake (delete)
        {
            var result = new Result<object>
            {
                Error = 0,
                Message = "Checked",
                Data = await _bookingService.CheckBooking(bookingId, Check)
            };

            return result;
        }

        [HttpPost("AddStylistToBooking")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<Result<object>> AddStylistToBooking(Guid bookingId, Guid stylistId)
        {
            var booking = await _bookingService.GetBookingById(bookingId);

            var result = new Result<object>
            {
                Error = 0,
                Message = "",
                Data = null
            };

            if (booking == null)
            {
                result.Error = 1;
                result.Message = "Booking is not found";
                return result;
            }

            var stylist = await _salonMemberService.GetSalonMemberById(stylistId);
            
            if (stylist == null)
            {
                result.Error = 1;
                result.Message = "Stylist is not found";
                return result;
            }

            booking.SalonMemberId = stylistId;
            booking.SalonMember = stylist;

            if (!await _bookingService.UpdateBooking(booking))
            {
                result.Error = 1;
                result.Message = "Add Stylist fail";
                return result;
            }

            BookingDTO bookingDTO = new BookingDTO()
            {
                BookingDate = booking.BookingDate,
                Checked = booking.Checked,
                TotalMoney = booking.TotalMoney,
            };

            result.Message = "Add Stylist successfully";
            result.Data = bookingDTO;

            return result;
        }

        [HttpPost("AddBooking")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<Result<object>> AddBooking([FromForm] Guid CustomerId, SalonDTO salonDto, Guid SalonMemberId, DateTime cuttingDate, Guid ServiceId, string ComboServiceId)
        {
            Decimal TotalAmount = 0;
            Booking booking = new Booking();

            var Result = new Result<object>
            {
                Error = 1,
                Message = "",
                Data = null
            };

            booking.Checked = false;
            booking.BookingDate = cuttingDate;
            booking.SalonMemberId = SalonMemberId;
            booking.UserId = CustomerId;
            booking.CreationDate = DateTime.Now;

            var salonMember = await _salonMemberService.GetSalonMemberById(SalonMemberId);

            if (salonMember != null)
            {
                booking.SalonMember = salonMember;

                booking.SalonMemberId = SalonMemberId;
            }

            var serviceObj = await _serviceService.GetServiceById(ServiceId);

            if (serviceObj.Data == null)
            {
                Result.Error = 1;
                Result.Message = "Service not found";
                return Result;
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

                    foreach (var item in booking.Service.ServiceComboServices)
                    {
                        item.ComboService = combo;
                    }

                    TotalAmount += combo.Price;
                }
            }

            if (TotalAmount == 0)
            {
                Result.Error = 1;
                Result.Message = "Combo service not found";
                return Result;
            }

            var Salon = await _salonService.SearchSalonById(salonDto);

            if (Salon.Data != null)
            {
                foreach (var item in booking.Service.ServiceComboServices)
                {
                    item.ComboService.SalonId = salonDto.SalonId;
                    item.ComboService.Salon = (Salon)Salon.Data;
                }
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
                Result.Error = 1;
                Result.Message = "Create booking faild";
                return Result;
            }

            var bookingDTO = _mapper.Map<BookingDTO>(booking);

            Result.Data = bookingDTO;   

            return Result;
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
