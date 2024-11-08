using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Auth
{
    public class RedisSessionDTO
    {
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public string? Refresh { get; set; }
    }
    public class LogoutRequestDTO
    {
        public required string RefreshToken { get; set; }
    }
}
