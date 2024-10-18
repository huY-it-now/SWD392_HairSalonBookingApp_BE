using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Salon;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISalonService
    {
        Task<Result<object>> CreateSalon(CreateSalonDTO req);
        Task<Result<object>> PrintAllSalon();
        Task<Salon> GetSalonById(Guid Id);
        Task<Result<object>> SearchSalonWithAddress(SalonDTO req);
    }
}
