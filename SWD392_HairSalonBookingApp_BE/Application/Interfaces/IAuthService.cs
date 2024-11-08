using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        public Task<GoogleAuthResponsesDTO> HandleGoogleLogin(string rdc, GoogleAuthInfoDTO info);
        public Task<IActionResult> HandleRefresh(RefreshRequestDTO req);
        public Task<IActionResult> HandleLogout(LogoutRequestDTO req);
    }
}
