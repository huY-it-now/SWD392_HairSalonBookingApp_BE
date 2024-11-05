﻿using Application.Interfaces;
using Application.Repositories;
using AutoMapper;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Booking;
using Domain.Contracts.DTO.Combo;
using Domain.Contracts.DTO.Salon;
using Domain.Contracts.DTO.User;
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
        private readonly IScheduleWorkTimeRepository _scheduleWorkTimeRepository;
        private readonly IPaymentLogRepository _paymentLogRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IPaymentStatusRepository _paymentStatusRepository;
        private readonly ISalonMemberScheduleRepository _salonMemberScheduleRepository;
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IComboServiceRepository _comboService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IMapper mapper, IComboServiceRepository comboService, IPaymentsRepository paymentsRepository, ISalonMemberScheduleRepository salonMemberScheduleRepository, IPaymentLogRepository paymentLogRepository, IPaymentMethodRepository paymentMethodRepository, IPaymentStatusRepository paymentStatusRepository, IScheduleWorkTimeRepository scheduleWorkTimeRepository)
        {
            _scheduleWorkTimeRepository = scheduleWorkTimeRepository;
            _paymentLogRepository = paymentLogRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _paymentStatusRepository = paymentStatusRepository;
            _salonMemberScheduleRepository = salonMemberScheduleRepository;
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

        public async Task<string> CheckBooking(Guid bookingId, bool Check)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return "Booking is not found";
            }

            if (Check)
            {
                booking.Checked = true;

                _bookingRepository.Update(booking);

                var cus = await _unitOfWork.UserRepository.GetUserById(booking.UserId);

                if (cus == null)
                {
                    return "Customer is not found";
                }

                Appointment appointment = new();
                appointment.AppointmentDate = booking.BookingDate;
                appointment.Stylist = booking.SalonMember;
                appointment.StylistId = booking.SalonMemberId;
                appointment.UserId = cus.Id;
                appointment.User = cus;
                appointment.ComboService = booking.ComboService;
                appointment.ComboServiceId = booking.ComboServiceId;
                appointment.Status = "Hadn't done";

                await _unitOfWork.AppointmentRepository.AddAsync(appointment);
            }
            else
            {
                _bookingRepository.SoftRemove(booking);
            }

            if (!(await _unitOfWork.SaveChangeAsync() > 0))
            {
                return "Save change fail";
            }

            return "Check success";
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

        public async Task<Result<object>> CreateBookingWithRequest(Guid CustomerId, Guid salonId, Guid SalonMemberId, DateTime cuttingDate, Guid ComboServiceId, string CustomerName, string CustomerPhoneNumber)
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
            booking.CustomerName = CustomerName;
            booking.CustomerPhoneNumber = CustomerPhoneNumber;

            var salonMember = await _unitOfWork.SalonMemberRepository.GetByIdAsync(SalonMemberId);

            if (salonMember == null)
            {
                var stylistListFree = await _unitOfWork.SalonMemberRepository.GetSalonMembersFree(booking.BookingDate, booking.salon);

                booking.SalonMember = stylistListFree.ElementAt(new Random().Next(stylistListFree.Count));
            }
            else
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

            var schedule = await _scheduleWorkTimeRepository.GetByTime(booking.BookingDate.Year, booking.BookingDate.Month, booking.BookingDate.Day);

            if (schedule == null)
            {
                var salonMemberSchedule = await _salonMemberScheduleRepository.GetByTime(booking.BookingDate.Year, booking.BookingDate.Month, booking.BookingDate.Day);
                ScheduleWorkTime scheduleWorkTime = new();
                scheduleWorkTime.ScheduleDate = booking.BookingDate;
                scheduleWorkTime.WorkShifts = booking.ComboService.ComboServiceName;
                scheduleWorkTime.SalonMemberSchedule = salonMemberSchedule;
                scheduleWorkTime.SalonMemberScheduleId = salonMemberSchedule.Id;
                await _scheduleWorkTimeRepository.AddAsync(scheduleWorkTime);

                if (!(await _unitOfWork.SaveChangeAsync() > 0))
                {
                    Result.Error = 1;
                    Result.Message = "Create stylist schedule fail";
                }
            }
            else
            {
                TimeSpan comboTime = CheckComboSericeTime(comboService.ComboServiceName);

                TimeSpan timeEndPredict = new TimeSpan(booking.BookingDate.Hour + comboTime.Hours, booking.BookingDate.Minute + comboTime.Minutes, 0);

                foreach (var item in schedule) //check xem co trung lich ko
                {
                    if (item.ScheduleDate == booking.BookingDate)
                    {
                        Result.Error = 1;
                        Result.Message = "This time has been booked";
                    }

                    TimeSpan timeOfWork = CheckComboSericeTime(item.ScheduleDate.ToString());

                    if ((timeEndPredict.Hours - item.ScheduleDate.Hour) > 0 && (booking.BookingDate.Hour - item.ScheduleDate.Hour) < 0)
                    {
                        Result.Error = 1;
                        Result.Message = $"Please choose another time before {timeEndPredict.Hours - item.ScheduleDate.Hour} hour";
                        return Result;
                    }
                    else if ((timeEndPredict.Hours - item.ScheduleDate.Hour) == 0 && (booking.BookingDate.Hour - item.ScheduleDate.Hour) < 0 && (item.ScheduleDate.Minute - timeEndPredict.Minutes) < 0)
                    {
                        Result.Error = 1;
                        Result.Message = $"Please choose another time before 30 minutes";
                        return Result;
                    }
                    else if ((booking.BookingDate.Hour - item.ScheduleDate.Hour) > 0 && (booking.BookingDate.Hour - item.ScheduleDate.Hour + timeOfWork.Hours) < 0)
                    {
                        Result.Error = 1;
                        Result.Message = $"Please choose another time after {item.ScheduleDate.Hour + timeOfWork.Hours - booking.BookingDate.Hour} hour";
                        return Result;
                    }
                }

                var salonMemberSchedule = await _salonMemberScheduleRepository.GetByTime(booking.BookingDate.Year, booking.BookingDate.Month, booking.BookingDate.Day);

                ScheduleWorkTime scheduleWorkTime = new();
                scheduleWorkTime.ScheduleDate = booking.BookingDate;
                scheduleWorkTime.WorkShifts = booking.ComboService.ComboServiceName;
                scheduleWorkTime.SalonMemberSchedule = salonMemberSchedule;
                scheduleWorkTime.SalonMemberScheduleId = salonMemberSchedule.Id;
                await _scheduleWorkTimeRepository.AddAsync(scheduleWorkTime);

                if (!(await _unitOfWork.SaveChangeAsync() > 0))
                {
                    Result.Error = 1;
                    Result.Message = "Create stylist schedule fail";
                }
            }

            booking.ComboServiceId = comboService.Id;
            booking.ComboService = comboService;
            
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
            var paymentStatus = new PaymentStatus();
            var paymentMethod = new PaymentMethods();

            paymentMethod.MethodName = "Qr";
            paymentMethod.Description = "Pay through Qr";

            paymentStatus.StatusName = "Pending";
            paymentStatus.Description = "Waiting for pay";

            payment.PaymentAmount = comboService.Price;
            payment.BookingId = CustomerId;
            payment.Booking = booking;
            payment.PaymentStatus = paymentStatus;
            payment.PaymentMethods = paymentMethod;

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
            return await _bookingRepository.GetBookingByIdWithComboAndPayment(Id);
        }

        public async Task<bool> UpdateBooking(Booking booking)
        {
            _bookingRepository.Update(booking);

            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public TimeSpan CheckComboSericeTime(string comboServiceName)
        {
            switch (comboServiceName)
            {
                case "Cắt tóc + Nhuộm": return new TimeSpan(1, 30, 0); 
                case "Cắt tóc + Cạo mặt + Ráy tai": return new TimeSpan(1, 0, 0); 
                case "Cắt tóc + Gội đầu + Uốn tóc": return new TimeSpan(3, 30, 0); 
                case "Cắt tóc + Duỗi": return new TimeSpan(4, 30, 0); 
                default: return TimeSpan.Zero;
            }
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
                    //item.PaymentStatus = booking[i].Payments.PaymentSatus.StatusName;
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

        public async Task<Result<object>> AddFeedBack(Guid bookingId, string FeedBack)
        {
            var result = new Result<object>
            {
                Error = 0,
                Message = "",
                Data = null
            };

            var booking = await _bookingRepository.GetCheckedBookingInformation();

            return result;
        }

        public async Task<Result<object>> GetBookingDetail(Guid bookingId)
        {
            var booking = await _unitOfWork.BookingRepository.GetBookingDetail(bookingId);

            if (booking == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Do not have any booking"
                };
            }

            var result = _mapper.Map<BookingDTO>(booking);

            return new Result<object>
            {
                Error = 0,
                Message = "Booking Detail",
                Data = result
            };
        }
    }
}
