using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Auth
{
    public class TokenResponsesDTO
    {
        public string? access_token { get; set; }
        public string? refresh_token { get; set; }
        public int access_token_exp { get; set; }
        public int refresh_token_exp { get; set; }
    }
}
