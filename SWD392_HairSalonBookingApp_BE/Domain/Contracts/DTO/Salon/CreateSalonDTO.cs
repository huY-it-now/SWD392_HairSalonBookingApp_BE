using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Salon
{
    public class CreateSalonDTO
    {
        public string SalonName { get; set; }
        public string Address { get; set; }
        public IFormFile? Image { get; set; }
    }
}
