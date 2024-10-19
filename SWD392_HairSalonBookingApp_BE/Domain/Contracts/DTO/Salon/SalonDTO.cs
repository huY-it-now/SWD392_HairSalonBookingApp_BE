using Microsoft.AspNetCore.Http;

namespace Domain.Contracts.DTO.Salon
{
    public class SalonDTO
    {
        public Guid SalonId { get; set; }
        public string SalonName { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
    }
}
