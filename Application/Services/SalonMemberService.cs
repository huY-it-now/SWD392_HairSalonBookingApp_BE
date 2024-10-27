using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SalonMemberService : ISalonMemberService
    {
        private readonly ISalonMemberRepository _salonMemberRepository;

        public SalonMemberService(ISalonMemberRepository salonMemberRepository)
        {
            _salonMemberRepository = salonMemberRepository;
        }

        public async Task<SalonMember> GetSalonMemberById(Guid id)
        {
            return await _salonMemberRepository.GetByIdAsync(id);
        }

        public async Task<List<SalonMember>> GetAllStylists()
        {
            return await _salonMemberRepository.GetSalonMemberWithRole(5);
        }

        public async Task<List<SalonMember>> GetAllSalonStaff()
        {
            return await _salonMemberRepository.GetSalonMemberWithRole(4);
        }
    }
}
