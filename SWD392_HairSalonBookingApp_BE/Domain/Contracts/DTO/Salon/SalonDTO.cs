using Microsoft.AspNetCore.Http;

namespace Domain.Contracts.DTO.Salon
{
    public class SalonDTO
    {
        public string Address { get; set; }
        public string Province { get; set; }
        public string Image { get; set; }
    }
}
