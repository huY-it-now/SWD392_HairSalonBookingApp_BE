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
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IComboServiceRepository _comboService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IMapper mapper, IComboServiceRepository comboService, IPaymentsRepository paymentsRepository)
        {
            _paymentsRepository = paymentsRepository;
            _comboService = comboService;
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

                var cus = await _unitOfWork.UserRepository.GetUserById(booking.UserId);

                if (cus == null)
                {
                    return false;
                }

                Appointment appointment = new();
                appointment.AppointmentDate = booking.BookingDate;
                appointment.Stylist = booking.SalonMember;
                appointment.StylistId = booking.SalonMemberId;
                appointment.UserId = cus.Id;
                appointment.User = cus;
                appointment.ComboService = booking.ComboService;
                appointment.ComboServiceId = booking.ComboServiceId;

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

        public async Task<Result<object>> CreateBookingWithRequest(Guid CustomerId, Guid salonId, Guid SalonMemberId, DateTime cuttingDate, Guid ComboServiceId)
        {
            Booking booking = new Booking();

            var Result = new Result<object>
            {
                Error = 0,
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

                booking.SalonMemberId = salonMember.Id;
            }

            var comboService = await _comboService.GetComboServiceById(ComboServiceId);

            if (comboService == null)
            {
                Result.Error = 1;
                Result.Message = "Combo service is not found";
                return Result;
            }
            else
            {
                booking.ComboServiceId = comboService.Id;
                booking.ComboService = comboService;
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

            if (!await CreateBooking(booking))
            {
                Result.Error = 1;
                Result.Message = "Create booking faild";
                return Result;
            }

            var payment = new Payments();
            var paymentStatus = new PaymentSatus();

            paymentStatus.StatusName = "Pending";
            paymentStatus.Discription = "Waiting for pay";

            payment.PaymentAmount = comboService.Price;
            payment.BookingId = CustomerId;
            payment.Booking = booking;
            payment.PaymentSatus = paymentStatus;

            await _paymentsRepository.AddAsync(payment);

            if (!(await _unitOfWork.SaveChangeAsync() > 0))
            {
                Result.Error = 1;
                Result.Message = "Create payment faild";
                return Result;
            }

            var bookingDTO = _mapper.Map<BookingDTO>(booking);

            Result.Message = "Create success";
            Result.Data = bookingDTO;

            return Result;
        }

        public async Task<Booking> GetBookingById(Guid Id)
        {
            return await _bookingRepository.GetByIdAsync(Id);
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

        public async Task<List<ViewCheckedBookingDTO>> ShowAllCheckedBooking()
        {
            var booking = await _bookingRepository.GetCheckedBookingInformation();

            var checkedBooking = _mapper.Map<List<ViewCheckedBookingDTO>>(booking);

            foreach (var item in checkedBooking)
            {
                for (int i = 0; i < booking.Count; i++)
                {
                    item.Total = booking[i].ComboService.Price;
                    item.PaymentStatus = booking[i].Payments.PaymentSatus.StatusName;
                }
            }

            return checkedBooking;
        }

        public async Task<List<ViewUncheckBookingDTO>> ShowAllUncheckedBooking()
        {
            var booking = await _bookingRepository.GetUncheckBookingInformation();

            var uncheckBooking = _mapper.Map<List<ViewUncheckBookingDTO>>(booking);

            foreach (var item in uncheckBooking)
            {
                for(int i = 0; i < booking.Count; i++)
                {
                    item.Total = booking[i].ComboService.Price;
                }
            }

            return uncheckBooking;
        }
    }
}
