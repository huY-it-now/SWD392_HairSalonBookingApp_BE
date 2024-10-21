using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Account
{
    public class ResetPasswordDTO
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
