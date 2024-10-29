using Application.Commons;
using Application.Interfaces;
using Application.Repositories;
using Application.Utils;
using Application.Validations.Stylist;
using AutoMapper;
using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Account;
using Domain.Contracts.DTO.Appointment;
using Domain.Contracts.DTO.Booking;
using Domain.Contracts.DTO.Combo;
using Domain.Contracts.DTO.Stylist;
using Domain.Contracts.DTO.User;
using Domain.Entities;
using Org.BouncyCastle.Asn1.Ocsp;

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
                Phone = user.PhoneNumber,
                Address = user.Address,
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
                    Message = "This email already exist, Do you want to verify?",
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
                IsDeleted = false,
                Status = false
            };

            await _emailService.SendOtpMail(request.FullName, token, request.Email);
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<UserDTO>(user);
            result.Phone = request.PhoneNumber;

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
            verify.Status = true;
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Verify successfully!",
                Data = null
            };
        }

        public async Task<Result<object>> CreateStylist(CreateStylistDTO request)
        {
            var stylist = await _unitOfWork.UserRepository.GetUserById(request.UserId);
            var salon = await _unitOfWork.SalonRepository.GetByIdAsync(request.SalonId);

            if (stylist == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Not found User",
                    Data = null
                };
            }

            if (salon == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Salon is not found",
                    Data = null
                };
            }

            if (request.Job.ToLower().StartsWith("stylist"))
            {
                stylist.RoleId = 5;
            }
            else if (request.Job.StartsWith("manager"))
            {
                stylist.RoleId = 3;
            }
            else if (request.Job.StartsWith("staff"))
            {
                stylist.RoleId = 4;
            }
            else
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Job not found",
                    Data = null
                };
            }

            var salonMember = new SalonMember
            {
                Id = stylist.Id,
                UserId = stylist.Id,
                SalonId = salon.Id,
                Job = request.Job,
                Rating = "newbie",
                IsDeleted = false,
                Status = true
            };

            await _unitOfWork.SalonMemberRepository.AddAsync(salonMember);
            await _unitOfWork.SaveChangeAsync();
            var result = _mapper.Map<StylistDTO>(request);
            result.FullName = stylist.FullName;
            result.Email = stylist.Email;
            result.Job = salonMember.Job;
            result.Rating = salonMember.Rating;
            result.Status = salonMember.Status;

            return new Result<object>
            {
                Error = 0,
                Message = "Create stylist successfully",
                Data = result
            };
        }

        public async Task<Result<object>> PrintAllSalonMember()
        {
            var salonMember = await _unitOfWork.SalonMemberRepository.GetAllSalonMember();

            if (salonMember == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "List member is empty",
                    Data = null
                };
            }

            var result = _mapper.Map<List<StylistDTO>>(salonMember);

            return new Result<object>
            {
                Error = 0,
                Message = "Print all member",
                Data = result
            };
        }

        public async Task<Result<object>> GetSalonMemberWithRole(int roleId)
        {
            var salonMember = await _unitOfWork.SalonMemberRepository.GetSalonMemberWithRole(roleId);

            if (salonMember == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "No member with role",
                    Data = null
                };
            }

            var result = _mapper.Map<List<StylistDTO>>(salonMember);

            return new Result<object>
            {
                Error = 0,
                Message = "Print member",
                Data = result
            };
        }

        public async Task<Result<object>> RegisterWorkSchedule(RegisterWorkScheduleDTO request)
        {
            var validShifts = new List<string> { "Morning", "Afternoon", "Evening" };
            if (request.WorkShifts.Any(shift => !validShifts.Contains(shift)))
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Invalid work shift. Only 'Morning', 'Afternoon', or 'Evening' are allowed.",
                    Data = null
                };
            }

            if (request.WorkShifts.Count != request.WorkShifts.Distinct().Count())
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Duplicate work shifts are not allowed.",
                    Data = null
                };
            }

            if (request.WorkShifts.Count > 3)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "You can register up to 3 shifts per day.",
                    Data = null
                };
            }

            var schedule = await _unitOfWork.ScheduleRepository.GetScheduleByDateAsync(request.StylistId, request.ScheduleDate);

            if (schedule != null)
            {
                var duplicateShifts = request.WorkShifts.Intersect(schedule.WorkShifts).ToList();
                if (duplicateShifts.Any())
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = $"The following shifts have already been registered for this date: {string.Join(", ", duplicateShifts)}.",
                        Data = null
                    };
                }

                if (schedule.WorkShifts.Count + request.WorkShifts.Count > 3)
                {
                    return new Result<object>
                    {
                        Error = 1,
                        Message = "You can register up to 3 shifts per day.",
                        Data = null
                    };
                }

                schedule.WorkShifts.AddRange(request.WorkShifts);
            }
            else
            {
                schedule = new SalonMemberSchedule
                {
                    SalonMemberId = request.StylistId,
                    ScheduleDate = request.ScheduleDate,
                    WorkShifts = request.WorkShifts
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


        public async Task<List<StylistDTO>> GetAvailableStylists(DateTime bookingTime)
        {
            var shift = WorkShiftDTO.GetAvailableShifts().FirstOrDefault(s => bookingTime.TimeOfDay >= s.StartTime && bookingTime.TimeOfDay < s.EndTime);
            if (shift == null)
            {
                return new List<StylistDTO>();
            }

            var availableStylists = await _unitOfWork.ScheduleRepository.GetAvailableStylistsByTime(shift.Shift, bookingTime.Date);

            var stylistDTOs = availableStylists.Select(stylist => new StylistDTO
            {
                Id = stylist.Id,
                FullName = stylist.FullName,
                Email = stylist.Email,
                Job = stylist.Job,
                Rating = stylist.Rating
            }).ToList();

            return stylistDTOs;
        }

        public async Task<List<WorkAndDayOffScheduleDTO>> ViewWorkAndDayOffSchedule(Guid stylistId, DateTime fromDate, DateTime toDate)
        {
            var schedules = await _unitOfWork.ScheduleRepository.GetSchedulesByUserIdAndDateRange(stylistId, fromDate, toDate);

            List<WorkAndDayOffScheduleDTO> scheduleList = new List<WorkAndDayOffScheduleDTO>();

            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                var schedule = schedules.FirstOrDefault(s => s.ScheduleDate.Date == date.Date);
                scheduleList.Add(new WorkAndDayOffScheduleDTO
                {
                    Date = date,
                    IsDayOff = schedule?.IsDayOff ?? true,
                    WorkShifts = schedule?.WorkShifts ?? new List<string>()
                });
            }

            return scheduleList;
        }


        public async Task<Result<object>> UpdateProfile(UpdateProfileDTO request)
        {
            var userExist = await _unitOfWork.UserRepository.GetUserById(request.Id);

            if (userExist == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Not found",
                    Data = null
                };
            }

            userExist.FullName = request.FullName;
            userExist.PhoneNumber = request.PhoneNumber;
            userExist.Address = request.Address;

            if (request.Email != userExist.Email)
            {
                var token = _emailService.GenerateRandomNumber();
                await _emailService.SendOtpMail(request.FullName, token, request.Email);
                userExist.VerifiedAt = null;
                userExist.VerificationToken = token;
                userExist.Email = request.Email;
            }

            _unitOfWork.UserRepository.Update(userExist);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<UpdateProfileDTO>(userExist);

            return new Result<object>
            {
                Error = 0,
                Message = "Update profile successfully",
                Data = result
            };
        }

        public async Task<Result<object>> ForgotPassword(string email)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);

            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Not found user"
                };
            }

            var token = _emailService.GenerateRandomNumber();
            await _emailService.SendOtpMail(user.FullName, token, user.Email);

            user.VerificationToken = token;
            user.VerifiedAt = null;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Change Password successfully"
            };
        }

        public async Task<Result<object>> ResetPassword(ResetPasswordDTO request)
        {
            var user = await _unitOfWork.UserRepository.Verify(request.Token);

            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Invalid token"
                };
            }

            _passwordHash.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.VerificationToken = null;
            user.VerifiedAt = DateTime.Now;

            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Reset Password successfully"
            };
        }

        public async Task<List<AppointmentDTO>> ViewAppointments(Guid stylistId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                if (stylistId == Guid.Empty)
                {
                    throw new ArgumentException("Stylist ID cannot be empty.");
                }

                if (fromDate > toDate)
                {
                    throw new ArgumentException("From date cannot be greater than to date.");
                }

                // Lấy danh sách các lịch hẹn theo stylist và ngày
                var appointments = await _unitOfWork.AppointmentRepository
                    .GetAppointmentsByStylistIdAndDateRange(stylistId, fromDate, toDate);

                return _mapper.Map<List<AppointmentDTO>>(appointments);
            }
            catch (Exception ex)
            {
                // Log the exception tại đây
                throw new ApplicationException("An error occurred while viewing appointments.", ex);
            }
        }

        public async Task<Result<object>> UpdateAppointmentStatus(UpdateAppointmentStatusDTO request)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetAppointmentByIdAsync(request.AppointmentId);
            if (appointment == null)
            {
                return new Result<object> { Error = 1, Message = "Appointment not found.", Data = null };
            }

            appointment.Status = request.Status;
            await _unitOfWork.SaveChangeAsync();

            return new Result<object> { Error = 0, Message = "Appointment status updated successfully.", Data = null };
        }

        public async Task<Result<object>> DeleteWorkShift(Guid stylistId, DateTime scheduleDate, string workShift)
        {
            var schedule = await _unitOfWork.ScheduleRepository.GetScheduleByDateAsync(stylistId, scheduleDate);
            if (schedule == null || !schedule.WorkShifts.Contains(workShift))
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "The specified work shift is not registered.",
                    Data = null
                };
            }

            schedule.WorkShifts.Remove(workShift);
            if (schedule.WorkShifts.Count == 0)
            {
                await _unitOfWork.ScheduleRepository.DeleteWorkShiftAsync(schedule);
            }

            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Work shift deleted successfully!",
                Data = null
            };
        }

        public async Task<Result<object>> GetBookingsByUserId(Guid userId)
        {
            var bookings = await _unitOfWork.UserRepository.GetBookingsByUserId(userId);

            if (bookings == null || bookings.Count == 0)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "You don't have any orders",
                    Data = null
                };
            }

            var bookingDTOs = bookings.Select(b => new BookingDTO
            {
                Id = b.Id,
                BookingDate = b.BookingDate,
                Checked = b.Checked,
                ComboServiceName = new List<ComboServiceForBookingDTO>
                {
                     new ComboServiceForBookingDTO
                        {
                    Id = b.ComboService.Id,
                    ComboServiceName = b.ComboService.ComboServiceName,
                    Price = b.ComboService.Price,
                    Image = b.ComboService.ImageUrl
                        }
                },
            }).ToList();

            var bookingUserDTO = new BookingUserDTO
            {
                UserId = userId,
                Bookings = bookingDTOs
            };

            return new Result<object>
            {
                Error = 0,
                Message = "Orders",
                Data = bookingUserDTO
            };
        }
    }
}
