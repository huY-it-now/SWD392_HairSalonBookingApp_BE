using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Repositories;
using AutoMapper;
using Domain.Contracts.DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ICacheService _cacheService;
        private readonly IUserRepository _userRepo;

        public Task<GoogleAuthResponsesDTO> HandleGoogleLogin(string rdc, GoogleAuthInfoDTO info)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> HandleLogout(LogoutRequestDTO req)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> HandleRefresh(RefreshRequestDTO req)
        {
            throw new NotImplementedException();
        }
    }
}
