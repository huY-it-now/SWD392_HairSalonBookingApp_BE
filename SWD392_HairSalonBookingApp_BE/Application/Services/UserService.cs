﻿using Application.Commons;
using Application.Interfaces;
using Application.Utils;
using AutoMapper;
using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Account;
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

        public async Task<Result<object>> CreateStylist(CreateStylistDTO request) {
            var stylist = await _unitOfWork.UserRepository.GetUserById(request.UserId);
            var salon = await _unitOfWork.SalonRepository.GetByIdAsync(request.SalonId);

            if (stylist == null) {
                return new Result<object> {
                    Error = 1,
                    Message = "Not found User",
                    Data = null
                };
            }

            if (salon == null) {
                return new Result<object> {
                    Error = 1,
                    Message = "Salon is not found",
                    Data = null
                };
            }

            if (request.Job.ToLower().StartsWith("stylist")) {
                stylist.RoleId = 5;
            } else if (request.Job.StartsWith("manager")) {
                stylist.RoleId = 3;
            } else if (request.Job.StartsWith("staff")) {
                stylist.RoleId = 4;
            } else {
                return new Result<object> {
                    Error = 1,
                    Message = "Job not found",
                    Data = null
                };
            }

            var salonMember = new SalonMember {
                Id = Guid.NewGuid(),
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

            return new Result<object> {
                Error = 0,
                Message = "Create stylist successfully",
                Data = result
            };
        }

        public async Task<Result<object>> PrintAllSalonMember() {
            var salonMember = await _unitOfWork.SalonMemberRepository.GetAllSalonMember();

            if (salonMember == null) {
                return new Result<object> {
                    Error = 1,
                    Message = "List member is empty",
                    Data = null
                };
            }

            var result = _mapper.Map<List<StylistDTO>>(salonMember);

            return new Result<object> {
                Error = 0,
                Message = "Print all member",
                Data = result
            };
        }

        public async Task<Result<object>> GetSalonMemberWithRole(int roleId) {
            var salonMember = await _unitOfWork.SalonMemberRepository.GetSalonMemberWithRole(roleId);

            if (salonMember == null) {
                return new Result<object> {
                    Error = 1,
                    Message = "No member with role",
                    Data = null
                };
            }

            var result = _mapper.Map<List<StylistDTO>>(salonMember);

            return new Result<object> {
                Error = 0,
                Message = "Print member",
                Data = result
            };
        }
    }
}
