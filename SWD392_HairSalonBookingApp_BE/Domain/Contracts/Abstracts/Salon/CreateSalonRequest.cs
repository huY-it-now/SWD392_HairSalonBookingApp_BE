using Microsoft.AspNetCore.Http;

namespace Domain.Contracts.Abstracts.Salon
{
    public class CreateSalonRequest
    {
        public string Address { get; set; }
        public IFormFile? Image { get; set; }
    }
}
