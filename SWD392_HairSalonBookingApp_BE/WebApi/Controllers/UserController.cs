using Application;
using Application.Commons;
using Application.Interfaces;
using Application.Services;
using Application.Utils;
using Application.Validations.Account;
using AutoMapper;
using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Account;
using Domain.Contracts.DTO.Appointment;
using Domain.Contracts.DTO.Feedback;
using Domain.Contracts.DTO.Stylist;
using Domain.Contracts.DTO.User;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Ocsp;
using WebAPI.Controllers;

namespace WebApi.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHash _passwordHash;
        private readonly IEmailService _emailService;
        private readonly AppConfiguration _configuration;
        private readonly ICurrentTime _currentTime;
        private readonly IGoogleAuthService _googleAuthService;

        public UserController(IUserService userService, IMapper mapper, IUnitOfWork unitOfWork, IPasswordHash passwordHash, IEmailService emailService, AppConfiguration configuration,
                           ICurrentTime currentTime, IGoogleAuthService googleAuthService)
        {
            _userService = userService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordHash = passwordHash;
            _emailService = emailService;
            _configuration = configuration;
            _currentTime = currentTime;
            _googleAuthService = googleAuthService;
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
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(request.Email);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (user.IsDeleted == true)
            {
                return BadRequest("You were banned by Admin");
            }

            var isPasswordValid = _passwordHash.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordValid)
            {
                return BadRequest("Incorrect password.");
            }

            if (user.VerifiedAt == null)
            {
                return BadRequest("Please verify your email.");
            }

            var token = user.GenerateJsonWebToken(_configuration.JWTSecretKey, _currentTime.GetCurrentTime());

            return Ok(token);
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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> HistoryBookingForUser(Guid userId)
        {
            var result = await _userService.GetBookingsByUserId(userId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedbackForUser(Guid bookingId, string feedback)
        {
            var result = await _userService.AddFeedbackForUser(bookingId, feedback);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UserFeedback(FeedbackDTO request)
        {
            var booking = await _unitOfWork.BookingRepository.GetBookingByIdAsync(request.BookingId);

            if (booking == null)
            {
                return BadRequest("Not found booking");
            }

            if (booking.FeedbackId != null)
            {
                return BadRequest("You have already feedback for this booking");
            }

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
            };

            await _unitOfWork.FeedbackRepository.AddAsync(feedback);

            booking.FeedbackId = feedback.Id;

            _unitOfWork.BookingRepository.Update(booking);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<ListFeedbackDTO>(booking);

            return Ok(result);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] string firebaseToken)
        {
            var userId = await _googleAuthService.VerifyFirebaseTokenAsync(firebaseToken);

            var userGuid = Guid.Parse(userId);

            var user = await _userService.GetUserById(userGuid);

            if (user == null)
            {
                // Nếu user chưa tồn tại trong database, tạo mới user
                await _userService.CreateUserAsync(new UserDTO { Id = userGuid });
            }

            var systemToken = await _googleAuthService.GenerateSystemTokenAsync(userGuid.ToString());
            return Ok(new { Token = systemToken });
        }

        [HttpGet]
        public async Task<IActionResult> ViewBookingByUserId(Guid userId)
        {
            var result = await _userService.GetBookingUnCompletedByUserId(userId);

            return Ok(result);
        }
    }
}

