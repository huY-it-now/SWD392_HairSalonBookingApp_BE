using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Salon;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISalonService
    {
        Task<Result<object>> CreateSalon(CreateSalonDTO req);
        Task<Result<object>> PrintAllSalon();
        Task<Result<object>> SearchSalonWithAddress(SalonDTO req);
        Task<Result<object>> SearchSalonById(SalonDTO req);
        Task<Result<object>> ViewSalonMemberBySalonId(ViewSalonDTO req);
        Task<Result<object>> ViewStylistBySalonId(Guid salonId);
    }
}
