using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Auth
{
    public class RefreshRequestDTO
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class RefreshResponsesDTO
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
