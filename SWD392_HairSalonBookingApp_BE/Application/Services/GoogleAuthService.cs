using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Utils;
using FirebaseAdmin.Auth;

namespace Application.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        public async Task<string> VerifyFirebaseTokenAsync(string firebaseToken)
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(firebaseToken);
            return decodedToken.Uid; // Trả về userId từ Firebase token
        }

        public async Task<string> GenerateSystemTokenAsync(string userId)
        {
            return JwtTokenGenerator.GenerateToken(userId);
        }
    }
}
