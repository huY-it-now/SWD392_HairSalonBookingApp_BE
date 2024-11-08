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
        public Task<GoogleAuthResponses> HandleGoogleLogin(string rdc, GgAuthInfo info);
        public Task<IActionResult> HandleRefresh(RefreshReq req);
        public Task<IActionResult> HandleLogout(LogoutReq req);
    }
}
