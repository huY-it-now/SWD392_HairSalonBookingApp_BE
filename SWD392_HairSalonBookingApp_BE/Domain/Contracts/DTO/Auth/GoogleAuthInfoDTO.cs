using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Auth
{
    public class GoogleAuthInfoDTO
    {
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileUrl { get; set; }
    }
    public class GoogleAuthResponsesDTO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? RedirectLink { get; set; }
    }
}
