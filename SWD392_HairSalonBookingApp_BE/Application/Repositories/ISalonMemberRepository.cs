using Application.Repositories;
using Domain.Entities;

public interface ISalonMemberRepository : IGenericRepository<SalonMember> {
    Task<List<SalonMember>> GetAllSalonMember();
    Task<List<SalonMember>> GetSalonMemberWithRole(int roleId);
}
