using Application.Interfaces;
using AutoMapper;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Appointment;
using Domain.Contracts.DTO.Stylist;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace WebApi.Controllers
{
    public class StylistController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public StylistController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("create-stylist")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> CreateStylist(CreateStylistRequest request)
        {
            var validator = new CreateStylistRequestValidation();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Validation failed!",
                    Data = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var mapper = _mapper.Map<CreateStylistDTO>(request);
            var result = await _userService.CreateStylist(mapper);

            return Ok(result);
        }

        [HttpPost("register-work-schedule")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> RegisterWorkSchedule([FromBody] RegisterWorkScheduleDTO request)
        {
            var result = await _userService.RegisterWorkSchedule(request);
            return Ok(result);
        }

        [HttpGet("view-work-and-day-off-schedule")]
        [ProducesResponseType(200, Type = typeof(List<WorkAndDayOffScheduleDTO>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewWorkAndDayOffSchedule([FromQuery] Guid stylistId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var result = await _userService.ViewWorkAndDayOffSchedule(stylistId, fromDate, toDate);

            return Ok(result);

        }

        [HttpGet("view-appointments")]
        [ProducesResponseType(200, Type = typeof(List<AppointmentDTO>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewAppointments([FromQuery] Guid stylistId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var result = await _userService.ViewAppointments(stylistId, fromDate, toDate);
            return Ok(result);
        }


        [HttpPost("update-appointment-status")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> UpdateAppointmentStatus([FromBody] UpdateBookingStatusDTO request)
        {
            var result = await _userService.UpdateAppointmentStatus(request);
            return Ok(result);
        }

        [HttpGet("get-available-stylists")]
        [ProducesResponseType(200, Type = typeof(List<StylistDTO>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetAvailableStylist([FromQuery]Guid salonId, [FromQuery] DateTime bookingDate, [FromQuery] int bookingHour, [FromQuery] int bookingMinute)
        {
            var bookingTime = new TimeSpan(bookingHour, bookingMinute, 0);

            var result = await _userService.GetAvailableStylists(salonId, bookingDate, bookingTime);
            return Ok(result);
        }

        [HttpPost("delete-work-shift")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> DeleteWorkShift([FromBody] Guid stylistId, DateTime scheduleDate, string workShift)
        {
            var result = await _userService.DeleteWorkShift(stylistId, scheduleDate, workShift);
            return Ok(result);
        }
    }
}

