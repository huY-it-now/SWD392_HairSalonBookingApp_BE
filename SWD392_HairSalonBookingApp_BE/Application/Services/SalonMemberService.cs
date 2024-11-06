using Application.Interfaces;
using Application.Repositories;
using AutoMapper;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Booking;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SalonMemberService : ISalonMemberService
    {
        private readonly ISalonMemberRepository _salonMemberRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SalonMemberService(ISalonMemberRepository salonMemberRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _salonMemberRepository = salonMemberRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<Result<object>> GetAllBookingForStylist(Guid stylistId)
        {
            var stylistBookings = await _unitOfWork.BookingRepository.GetBookingForStylist(stylistId);

            if (stylistBookings == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Not found stylist"
                };
            }

            var result = _mapper.Map<List<BookingDTO>>(stylistBookings);

            return new Result<object>
            {
                Error = 0,
                Message = "List booking for stylist",
                Data = result
            };
        }
    }
}
