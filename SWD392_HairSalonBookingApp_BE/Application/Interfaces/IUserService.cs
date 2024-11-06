using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Account;
using Domain.Contracts.DTO.Appointment;
using Domain.Contracts.DTO.Booking;
using Domain.Contracts.DTO.Feedback;
using Domain.Contracts.DTO.Stylist;
using Domain.Contracts.DTO.User;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<object>> GetAllUser();
        Task<Result<object>> Register(RegisterUserDTO request);
        Task<Result<object>> Verify(VerifyTokenDTO request);
        Task<Result<object>> GetUserById(Guid id);
        Task<Result<object>> CreateStylist(CreateStylistDTO request);
        Task<Result<object>> PrintAllSalonMember();
        Task<Result<object>> GetSalonMemberWithRole(int roleId);
        Task<Result<object>> RegisterWorkSchedule(RegisterWorkScheduleDTO request);
        Task<List<StylistDTO>> GetAvailableStylists(Guid salonId, DateTime bookingDate, TimeSpan bookingTime);
        Task<List<WorkAndDayOffScheduleDTO>> ViewWorkAndDayOffSchedule(Guid stylistId, DateTime fromDate, DateTime toDate);
        Task<Result<object>> UpdateProfile(UpdateProfileDTO request);
        Task<Result<object>> ForgotPassword(string email);
        Task<Result<object>> ResetPassword(ResetPasswordDTO request);
        Task<List<BookingDTO>> ViewAppointments(Guid stylistId, DateTime fromDate, DateTime toDate);
        Task<Result<object>> UpdateBookingStatus(UpdateBookingStatusDTO request);
        Task<Result<object>> DeleteWorkShift(Guid stylistId, DateTime scheduleDate, string workShift);
        Task<Result<object>> GetBookingsByUserId(Guid userId);
        Task<Result<object>> GetAdminDashboard();
        Task<Result<object>> GetAllStaff();
        Task<Result<object>> GetAllManager();
        Task<Result<object>> AddFeedbackForUser(Guid bookingId, string feedback);
        Task<Result<object>> BanUser(Guid userId);
        Task<Result<object>> GetBookingUnCompletedByUserId(Guid userId);
        Task<string> UserFeedback(FeedbackDTO request);
        Task<Result<object>> GetListFeedback();

        Task<Result<object>> GetUserByEmailAsync(string email);
        Task<Result<object>> CreateUserAsync(UserDTO userDto);

    }
}
