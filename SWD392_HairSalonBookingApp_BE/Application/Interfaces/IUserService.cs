using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Account;
using Domain.Contracts.DTO.Stylist;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<object>> GetAllUser();
        Task<Result<object>> Register(RegisterUserDTO request);
        Task<Result<object>> Login(LoginUserDTO request);
        Task<Result<object>> Verify(VerifyTokenDTO request);
        Task<Result<object>> GetUserById(Guid id);
        Task<Result<object>> CreateStylist(CreateStylistDTO request);
        Task<Result<object>> PrintAllSalonMember();
        Task<Result<object>> GetSalonMemberWithRole(int roleId);
        Task<Result<object>> ViewWorkAndDayOffSchedule (Guid StylistId, DateTime fromDate, DateTime toDate);
        Task<Result<object>> RegisterWorkSchedule(RegisterWorkScheduleDTO request);
        Task<Result<object>> RegisterDayOff(RegisterDayOffDTO request);
        Task<Result<object>> UpdateProfile(UpdateProfileDTO request);
    }
}
