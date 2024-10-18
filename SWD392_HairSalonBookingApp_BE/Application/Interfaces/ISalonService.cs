using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Salon;

namespace Application.Interfaces
{
    public interface ISalonService
    {
        Task<Result<object>> CreateSalon(CreateSalonDTO req);
        Task<Result<object>> PrintAllSalon();
        Task<Result<object>> SearchSalonWithAddress(SalonDTO req);
    }
}
