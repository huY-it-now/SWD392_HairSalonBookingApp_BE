using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<string> VerifyFirebaseTokenAsync(string firebaseToken);
        Task<string> GenerateSystemTokenAsync(string userId);
    }
}
