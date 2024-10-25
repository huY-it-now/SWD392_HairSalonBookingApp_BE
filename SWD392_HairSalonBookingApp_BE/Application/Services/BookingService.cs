using Application.Interfaces;
using Application.Repositories;
using AutoMapper;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Booking;
using Domain.Contracts.DTO.Salon;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingDTO> AddRandomStylist(Guid Id)
        {
            var booking = await _bookingRepository.GetByIdAsync(Id);

            var stylistListFree = await _unitOfWork.SalonMemberRepository.GetSalonMembersFree(booking.BookingDate, booking.salon);

            booking.SalonMember = stylistListFree.ElementAt(new Random().Next(stylistListFree.Count));

            return _mapper.Map<BookingDTO>(booking);
        }

        public async Task<bool> CheckBooking(Guid bookingId, bool Check)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return false;
            }

            if (Check)
            {
                booking.Checked = true;

                _bookingRepository.Update(booking);

                var cus = await _unitOfWork.CustomerRepository.GetByIdAsync(booking.UserId);

                if (cus == null)
                {
                    return false;
                }

                Appointment appointment = new();
                appointment.AppointmentDate = booking.BookingDate;
                appointment.Stylist = booking.SalonMember;
                appointment.StylistId = booking.SalonMemberId;
                appointment.CustomerId = cus.Id;
                appointment.Customer = cus;
                appointment.Service = booking.Service;
                appointment.ServiceId = booking.ServiceId;


                await _unitOfWork.AppointmentRepository.AddAsync(appointment);
            }
            else
            {
                _bookingRepository.SoftRemove(booking);
            }

            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> CreateBooking(Booking booking)
        {
            if (booking != null)
            {
                await _bookingRepository.AddAsync(booking);
            }
            else
            {
                return false;
            }

            return await  _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<SalonMember> ChooseRandomStylist(DateTime dateTime, Salon salon)
        {
            var stylistListFree = await _unitOfWork.SalonMemberRepository.GetSalonMembersFree(dateTime, salon);

            var salonMember = stylistListFree.ElementAt(new Random().Next(stylistListFree.Count));

            return salonMember;
        }

        public async Task<Result<object>> CreateBookingWithRequest(Guid CustomerId, Guid salonId, Guid SalonMemberId, DateTime cuttingDate, string ComboServiceId, Guid ServiceId)
        {
            Decimal TotalAmount = 0;
            Booking booking = new Booking();

            var Result = new Result<object>
            {
                Error = 1,
                Message = "",
                Data = null
            };

            booking.Checked = false;
            booking.BookingDate = cuttingDate;
            booking.SalonMemberId = SalonMemberId;
            booking.UserId = CustomerId;
            booking.CreationDate = DateTime.Now;

            var salonMember = await _unitOfWork.SalonMemberRepository.GetByIdAsync(SalonMemberId);

            if (salonMember != null)
            {
                booking.SalonMember = salonMember;

                booking.SalonMemberId = SalonMemberId;
            }

            foreach (var id in ExtractValidIds(ComboServiceId))
            {
                var comboService = await _unitOfWork.ComboServiceRepository.GetByIdAsync(new Guid(id));

                if (comboService != null)
                {
                    var combo = comboService;

                    foreach (var item in booking.Service.ServiceComboServices)
                    {
                        item.ComboService = combo;
                    }

                    TotalAmount += combo.Price;
                }
            }

            if (TotalAmount == 0)
            {
                Result.Error = 1;
                Result.Message = "Combo service not found";
                return Result;
            }

            var Salon = await _unitOfWork.SalonRepository.GetByIdAsync(salonId);

            if (Salon != null)
            {
                booking.salon = Salon;
                booking.SalonId = salonId;
            }
            else
            {
                Result.Error = 1;
                Result.Message = "Salon not found";
                return Result;
            }

            var service = await _unitOfWork.ServiceRepository.GetByIdAsync(ServiceId);

            if (service != null)
            {
                booking.Service = service;
                booking.ServiceId = ServiceId;
            }
            else
            {
                Result.Error = 1;
                Result.Message = "Salon not found";
                return Result;
            }

            if (string.IsNullOrEmpty(SalonMemberId.ToString()))
            {
                 var salonmember = await ChooseRandomStylist(cuttingDate, Salon);
                booking.SalonMember = salonmember;
                booking.SalonMemberId = salonmember.Id;
            }

            var User = await _unitOfWork.UserRepository.GetByIdAsync(CustomerId);

            if (User != null)
            {
                booking.User = User;
                booking.UserId = CustomerId;
            }

            booking.TotalMoney = TotalAmount;

            if (!await CreateBooking(booking))
            {
                Result.Error = 1;
                Result.Message = "Create booking faild";
                return Result;
            }

            var bookingDTO = _mapper.Map<BookingDTO>(booking);

            Result.Data = bookingDTO;

            return Result;
        }

        public async Task<Booking> GetBookingById(Guid Id)
        {
            return await _bookingRepository.GetByIdAsync(Id);
        }

        public async Task<List<BookingDTO>> ShowAllUncheckedBooking()
        {
            return _mapper.Map<List<BookingDTO>>(await _bookingRepository.GetFullBookingInformation());
        }

        public async Task<bool> UpdateBooking(Booking booking)
        {
            _bookingRepository.Update(booking);

            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        List<string> ExtractValidIds(string input)
        {
            List<string> result = new List<string>();
            int start = 0;

            while (start < input.Length)
            {
                bool found = false;

                // Try different lengths starting from the current position
                for (int length = 36; length <= input.Length - start; length++)
                {
                    string potentialGuid = input.Substring(start, length);

                    // Validate if the substring is a valid GUID
                    if (Guid.TryParse(potentialGuid, out _))
                    {
                        result.Add(potentialGuid);  // Add the valid GUID to the result list
                        start += length;            // Move start index to the end of the found GUID
                        found = true;
                        break;                      // Break out to find the next GUID
                    }
                }

                // If no valid GUID was found, increment the start position
                if (!found)
                {
                    start++;
                }
            }

            return result;
        }
    }
}
