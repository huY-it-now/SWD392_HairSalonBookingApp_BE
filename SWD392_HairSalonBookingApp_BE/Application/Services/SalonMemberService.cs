using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
