using Application.Commons;
using Application.Interfaces;
using Application.Utils;
using Application.Validations.Stylist;
using AutoMapper;
using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Account;
using Domain.Contracts.DTO.Stylish;
using Domain.Contracts.DTO.Stylist;
using Domain.Contracts.DTO.User;
using Domain.Entities;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHash _passwordHash;
        private readonly IEmailService _emailService;
        private readonly AppConfiguration _configuration;
        private readonly ICurrentTime _currentTime;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, IPasswordHash passwordHash, IEmailService emailService, AppConfiguration configuration, ICurrentTime currentTime)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordHash = passwordHash;
            _emailService = emailService;
            _configuration = configuration;
            _currentTime = currentTime;
        }

        public async Task<Result<object>> GetAllUser()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var userMapper = _mapper.Map<List<UserDTO>>(users);

            return new Result<object>
            {
                Error = 0,
                Message = "Print all users",
                Data = userMapper
            };
        }

        public async Task<Result<object>> GetUserById(Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(id);

            var userDTO = new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber
            };

            return new Result<object>
            {
                Error = 0,
                Message = "All user",
                Data = userDTO
            };
        }

        public async Task<Result<object>> Login(LoginUserDTO request)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(request.Email);

            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "User not found.",
                    Data = null
                };
            }

            var isPasswordValid = _passwordHash.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordValid)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Incorrect password.",
                    Data = null
                };
            }

            if (user.VerifiedAt == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Please verify your email.",
                    Data = null
                };
            }

            var token = user.GenerateJsonWebToken(_configuration.JWTSecretKey, _currentTime.GetCurrentTime());

            return new Result<object>
            {
                Error = 0,
                Message = $"Welcome back, {user.FullName}!",
                Data = token
            };
        }


        public async Task<Result<object>> Register(RegisterUserDTO request)
        {
            var emailExist = await _unitOfWork.UserRepository.CheckEmailExist(request.Email);

            if (emailExist)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "This email already exists, please check again!",
                    Data = null
                };
            }

            _passwordHash.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var token = _emailService.GenerateRandomNumber();

            var user = new User
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = 2,
                VerificationToken = token,
            };

            await _emailService.SendOtpMail(request.FullName, token, request.Email);
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<UserDTO>(user);

            return new Result<object>
            {
                Error = 0,
                Message = "Register successfully! Please check mail to verify.",
                Data = result
            };
        }

        public async Task<Result<object>> Verify(VerifyTokenDTO request)
        {
            var verify = await _unitOfWork.UserRepository.Verify(request.Token);

            if (verify == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Invalid token",
                    Data = null
                };
            }

            verify.VerifiedAt = DateTime.UtcNow;
            verify.VerificationToken = null;
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Verify successfully!",
                Data = null
            };
        }

        public async Task<Result<object>> ViewWorkAndDayOffSchedule(Guid stylistId, DateTime fromDate, DateTime toDate)
        {
            var stylist = await _unitOfWork.UserRepository.GetUserById(stylistId);

            if (stylist == null || stylist.RoleId != 5)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Stylish not found!",
                    Data = null
                };
            }

            var schedules = await _unitOfWork.ScheduleRepository.GetSchedulesByUserIdAndDateRange(stylistId, fromDate, toDate);

            if (schedules == null || schedules.Count == 0)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "No schedules found within the given date range.",
                    Data = null
                };
            }

            var scheduleDTOs = _mapper.Map<List<ScheduleDTO>>(schedules);

            return new Result<object>
            {
                Error = 0,
                Message = "Work and day-off schedule retrieved successfully!",
                Data = scheduleDTOs
            };
        }

        public async Task<Result<object>> RegisterWorkSchedule(RegisterWorkScheduleDTO request)
        {
            var validation = new WorkDayValidation();
            var validationResult = validation.Validate(request);

            if (!validationResult.IsValid)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Validation failed!",
                    Data = validationResult.Errors.Select(x => x.ErrorMessage)
                };
            }

            var existingSchedule = await _unitOfWork.ScheduleRepository.GetSchedulesByUserIdAndDateRange(request.StylistId, request.ScheduleDate, request.ScheduleDate.AddDays(1));

            if (existingSchedule.Count + request.WorkShifts.Count > 3)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Cannot register more than 3 shifts for one day.",
                    Data = null
                };
            }

            foreach (var shift in request.WorkShifts)
            {
                var schedule = new SalonMemberSchedule
                {
                    StylistId = request.StylistId,
                    Date = request.ScheduleDate,
                    WorkShift = shift.ShiftName
                };

                await _unitOfWork.ScheduleRepository.AddAsync(schedule);
            }

            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Work schedule registered successfully!",
                Data = null
            };
        }

        public async Task<Result<object>> RegisterDayOff(RegisterDayOffDTO request)
        {
            var schedule = new SalonMemberSchedule
            {
                StylistId = request.StylistId,
                Date = request.DayOffDate,
                IsDayOff = true,
            };

            await _unitOfWork.ScheduleRepository.AddAsync(schedule);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Day off registered successfully!",
                Data = null
            };

        }

    }
}
