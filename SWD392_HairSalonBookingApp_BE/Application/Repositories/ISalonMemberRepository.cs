using Application.Repositories;
using Domain.Entities;

public interface ISalonMemberRepository : IGenericRepository<SalonMember> {
    Task<List<SalonMember>> GetAllSalonMember();
    Task<List<SalonMember>> GetSalonMemberWithRole(int roleId);
    Task<List<SalonMember>> GetSalonMemberBySalonId(Guid salonId);
    Task<List<SalonMember>> GetSalonMembersFree(DateTime dateTime, Guid salonId, int HourStart, int HourEnd, int minuteStart, int minutEnd, SalonMember salonMember);
    Task<List<SalonMember>> GetStylistBySalonId(Guid salonId);
    Task<SalonMember> GetSalonMemberById(Guid stylistId);
}
