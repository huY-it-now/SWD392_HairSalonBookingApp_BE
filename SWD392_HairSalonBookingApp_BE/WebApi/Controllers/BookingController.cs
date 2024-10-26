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

        [HttpPut("AddRandomStylist")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<Result<object>> AddRandomStylist(Guid bookingId)
        {
            var booking = await _bookingService.AddRandomStylist(bookingId);

            var result = new Result<object>
            {
                Error = 0,
                Message = "",
                Data = booking
            };

            if (booking == null)
            {
                result.Message = "Add random stylist fail";
            }
            else
            {
                result.Message = "Add random stylist success";
            }

            return result;
        }

        [HttpDelete("CacelBooking")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<Result<object>> CacelBooking(Guid bookingId)
        {
            var result = new Result<object>
            {
                Error = 0,
                Message = "Cancel completed",
                Data = await _bookingService.CheckBooking(bookingId, false)
            };

            return result;
        }

        [HttpPut("AddStylistToBooking")]
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
        public async Task<Result<object>> AddBooking([FromForm] Guid CustomerId, Guid salonId, Guid SalonMemberId, DateTime cuttingDate, int hour, int minute, Guid ComboServiceId)
        {
            var result = new Result<object>
            {
                Error = 0,
                Message = "",
                Data = null
            };

            DateTime dateTime = new DateTime(cuttingDate.Year, cuttingDate.Month, cuttingDate.Day, hour, minute, 0);

            if ((dateTime - DateTime.Now) < TimeSpan.FromHours(1))
            {
                result.Error = 1;
                result.Message = "Please booking at least 1 hour after now";
                return result;
            }

            if (string.IsNullOrEmpty(ComboServiceId.ToString()))
            {
                result.Error = 1;
                result.Message = "ComboService Id is null";
                return result;
            }

            result = await _bookingService.CreateBookingWithRequest(CustomerId, salonId,  SalonMemberId, dateTime, ComboServiceId);

            return result;
        }
    }
}
