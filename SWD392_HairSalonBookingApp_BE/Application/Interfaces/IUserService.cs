using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Account;
using Domain.Contracts.DTO.Appointment;
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
        Task<Result<object>> RegisterWorkSchedule(RegisterWorkScheduleDTO request);
        Task<List<StylistDTO>> GetAvailableStylists(DateTime bookingTime);
        Task<List<WorkAndDayOffScheduleDTO>> ViewWorkAndDayOffSchedule(Guid stylistId, DateTime fromDate, DateTime toDate);
        Task<Result<object>> UpdateProfile(UpdateProfileDTO request);
        Task<Result<object>> ForgotPassword(string email);
        Task<Result<object>> ResetPassword(ResetPasswordDTO request);
        Task<List<AppointmentDTO>> ViewAppointments(Guid stylistId, DateTime fromDate, DateTime toDate);
        Task<Result<object>> UpdateAppointmentStatus(UpdateAppointmentStatusDTO request);
        Task<Result<object>> DeleteWorkShift(Guid stylistId, DateTime scheduleDate, string workShift);
        Task<Result<object>> GetBookingsByUserId(Guid userId);
    }
}
