using Application.Interfaces;
using Application.Validations.Account;
using AutoMapper;
using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Account;
using Domain.Contracts.DTO.Appointment;
using Domain.Contracts.DTO.Stylist;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using WebAPI.Controllers;

namespace WebApi.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> PrintAllUser()
        {
            var users = await _userService.GetAllUser();

            return Ok(users);
        }

        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest req)
        {
            var validator = new RegisterUserValidator();
            var validatorResult = validator.Validate(req);
            if (validatorResult.IsValid == false)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Missing value!",
                    Data = validatorResult.Errors.Select(x => x.ErrorMessage),
                });
            }

            var result = await _userService.Register(_mapper.Map<RegisterUserDTO>(req));

            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest req)
        {
            var validator = new LoginUserValidator();
            var validationResult = validator.Validate(req);
            if (!validationResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Validation failed!",
                    Data = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var loginDto = _mapper.Map<LoginUserDTO>(req);
            var result = await _userService.Login(loginDto);

            return Ok(result);
        }

        [HttpPost("verify")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> Verify([FromBody] VerifyTokenRequest req)
        {
            var validator = new VerifyTokenValidator();
            var validationResult = validator.Validate(req);
            if (!validationResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Validation failed!",
                    Data = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var verifyMapper = _mapper.Map<VerifyTokenDTO>(req);
            var result = await _userService.Verify(verifyMapper);

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _userService.GetUserById(id);

            return Ok(result);
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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> PrintAllSalonMember()
        {
            var result = await _userService.PrintAllSalonMember();
            return Ok(result);
        }

        [HttpPost("get-member-with-role")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetMemberWithRole(int roleId)
        {
            var result = await _userService.GetSalonMemberWithRole(roleId);
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
        [HttpPost("udpate-profile")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> UpdateProfile([FromForm]  UpdateProfileRequest req)
        {
            var validator = new UpdateProfileValidation();
            var validatorResult = validator.Validate(req);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Validation failed!",
                    Data = validatorResult.Errors.Select(x => x.ErrorMessage),
                });
            }

            var userMapper = _mapper.Map<UpdateProfileDTO>(req);
            var result = await _userService.UpdateProfile(userMapper);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _userService.ForgotPassword(email);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequest req)
        {
            var validator = new ResetPasswordRequestValidator();
            var validatorResult = validator.Validate(req);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Validation failed!",
                    Data = validatorResult.Errors.Select(x => x.ErrorMessage),
                });
            }

            var mapper = _mapper.Map<ResetPasswordDTO>(req);
            var result = await _userService.ResetPassword(mapper);
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
        public async Task<IActionResult> UpdateAppointmentStatus([FromBody] UpdateAppointmentStatusDTO request)
        {
            var result = await _userService.UpdateAppointmentStatus(request);
            return Ok(result);
        }
    }
}
