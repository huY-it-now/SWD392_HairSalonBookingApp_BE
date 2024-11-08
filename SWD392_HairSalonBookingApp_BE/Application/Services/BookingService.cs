using Application.Interfaces;
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

        public BookingService(IBookingRepository bookingRepository, 
                              IUnitOfWork unitOfWork, 
                              IMapper mapper, 
                              IComboServiceRepository comboService, 
                              IPaymentsRepository paymentsRepository, 
                              ISalonMemberScheduleRepository salonMemberScheduleRepository, 
                              IPaymentLogRepository paymentLogRepository, 
                              IPaymentMethodRepository paymentMethodRepository, 
                              IPaymentStatusRepository paymentStatusRepository, 
                              IScheduleWorkTimeRepository scheduleWorkTimeRepository)
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
            var booking = await _bookingRepository.GetBookingByIdAsync(Id);

            var stylist = await ChooseRandomStylist(booking.BookingDate, booking.salon, booking.ComboService.ComboServiceName, booking.SalonMember);

            if (stylist == null)
            {
                return null;
            }

            booking.SalonMember = stylist;

            _bookingRepository.Update(booking);

            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<BookingDTO>(booking);
        }

        public async Task<string> CheckBooking(Guid bookingId, string Check)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return "Booking is not found";
            }

            if (Check != "Pending")
            {
                booking.BookingStatus = Check;

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

            return "Success";
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

        public async Task<SalonMember> ChooseRandomStylist(DateTime dateTime, Salon salon, string comboServiceName, SalonMember salonMember)
        {
            int EndHour = dateTime.Hour + CheckComboSericeTime(comboServiceName).Hours;

            var stylistListFree = await _unitOfWork.SalonMemberRepository.GetSalonMembersFree(dateTime, salon, dateTime.Hour, EndHour, 0, 0, salonMember);

            if (stylistListFree == null)
            {
                return null;
            }

            var salonMemberAdd = stylistListFree.ElementAt(new Random().Next(stylistListFree.Count));

            return salonMemberAdd;
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

            // Kiểm tra trùng lịch booking trước khi tạo mới
            var existingBooking = await _unitOfWork.BookingRepository.GetBookingBySalonAndDateAsync(salonId, cuttingDate);

            if (existingBooking != null)
            {
                Result.Error = 1;
                Result.Message = "This time has already been booked for the selected salon. Please choose a different time.";
                return Result;
            }

            booking.BookingStatus = "Pending";
            booking.BookingDate = cuttingDate;
            booking.SalonMemberId = SalonMemberId;
            booking.UserId = CustomerId;
            booking.CreationDate = DateTime.Now;
            booking.CustomerName = CustomerName;
            booking.CustomerPhoneNumber = CustomerPhoneNumber;

            var comboService = await _comboService.GetComboServiceById(ComboServiceId);

            if (comboService == null)
            {
                Result.Error = 1;
                Result.Message = "Combo service is not found";
                return Result;
            }

            var salonMember = await _unitOfWork.SalonMemberRepository.GetSalonMemberById(SalonMemberId);
            var Salon = await _unitOfWork.SalonRepository.GetSalonById(salonId);

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

            if (salonMember == null)
            {
                booking.SalonMember = await ChooseRandomStylist(cuttingDate, Salon, comboService.ComboServiceName, salonMember);

                if (booking.SalonMember == null)
                {
                    Result.Error = 1;
                    Result.Message = "There is no free stylist now.";
                    return Result;
                }
            }
            else
            {
                booking.SalonMember = salonMember;
                booking.SalonMemberId = salonMember.Id;
            }

            // Tiếp tục các bước để tạo lịch làm việc cho salon member...
            var salonMemberSchedule = await _salonMemberScheduleRepository.GetByTime(booking.BookingDate.Year, booking.BookingDate.Month, booking.BookingDate.Day);

            if (salonMemberSchedule == null)
            {
                List<string> list = new List<string> { "Check" };
                salonMemberSchedule = new SalonMemberSchedule
                {
                    Id = Guid.NewGuid(),
                    IsDayOff = false,
                    SalonMember = booking.SalonMember,
                    SalonMemberId = booking.SalonMemberId,
                    WorkShifts = list
                };

                await _salonMemberScheduleRepository.AddAsync(salonMemberSchedule);
                await _unitOfWork.SaveChangeAsync();
            }

            // Tạo lịch làm việc mới
            ScheduleWorkTime scheduleWorkTime = new ScheduleWorkTime
            {
                ScheduleDate = booking.BookingDate,
                WorkShifts = comboService.ComboServiceName,
                SalonMemberSchedule = salonMemberSchedule,
                SalonMemberScheduleId = salonMemberSchedule.Id
            };

            await _scheduleWorkTimeRepository.AddAsync(scheduleWorkTime);

            if (!(await _unitOfWork.SaveChangeAsync() > 0))
            {
                Result.Error = 1;
                Result.Message = "Create stylist schedule failed";
                return Result;
            }

            salonMemberSchedule.WorkShifts.Add(scheduleWorkTime.WorkShifts);

            booking.ComboServiceId = comboService.Id;
            booking.ComboService = comboService;

            var User = await _unitOfWork.UserRepository.GetByIdAsync(CustomerId);

            if (User != null)
            {
                booking.User = User;
                booking.UserId = CustomerId;
            }

            // Tạo booking mới
            if (!await CreateBooking(booking))
            {
                Result.Error = 1;
                Result.Message = "Create booking failed";
                return Result;
            }

            // Tạo thông tin thanh toán
            var payment = new Payments
            {
                PaymentAmount = comboService.Price,
                BookingId = booking.Id,
                Booking = booking,
                PaymentStatus = new PaymentStatus
                {
                    StatusName = "Pending",
                    Description = "Waiting for pay"
                },
                PaymentMethods = new PaymentMethods
                {
                    MethodName = "Qr",
                    Description = "Pay through Qr"
                },
                PaymentDate = DateTime.UtcNow
            };

            await _paymentsRepository.AddAsync(payment);

            booking.PaymentId = payment.Id;

            _bookingRepository.Update(booking);

            if (!(await _unitOfWork.SaveChangeAsync() > 0))
            {
                Result.Error = 1;
                Result.Message = "Create payment failed";
                return Result;
            }

            var bookingDTO = new BookingDTO
            {
                Id = booking.Id,
                PaymentId = payment.Id,
                BookingDate = booking.BookingDate,
                BookingStatus = booking.BookingStatus,
                CustomerName = booking.CustomerName,
                CustomerPhoneNumber = booking.CustomerPhoneNumber,
                SalonName = booking.salon.salonName,
                Address = booking.salon.Address,
                PaymentAmount = booking.ComboService.Price,
                PaymentDate = booking.Payments.PaymentDate,
                PaymentStatus = booking.Payments.PaymentStatus.StatusName,
                StylistId = booking.SalonMemberId,
            };
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
                    item.PaymentStatus = booking[i].Payments.PaymentStatus.StatusName;
                }
            }

            return checkedBooking;
        }

        public async Task<List<ViewPendingBookingDTO>> ShowAllPendingedBooking()
        {
            var booking = await _bookingRepository.GetPendingBookingInformation();

            var PendingBooking = _mapper.Map<List<ViewPendingBookingDTO>>(booking);

            foreach (var item in PendingBooking)
            {
                for(int i = 0; i < booking.Count; i++)
                {
                    item.Total = booking[i].ComboService.Price;
                }
            }

            return PendingBooking;
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
                    Message = "Do not found any booking"
                };
            }

            var result = new BookingDTO
            {
                Id = booking.Id,
                PaymentId = booking.PaymentId,
                BookingDate = booking.BookingDate,
                BookingStatus = booking.BookingStatus,
                CustomerName = booking.CustomerName,
                CustomerPhoneNumber = booking.CustomerPhoneNumber,
                Feedback = booking.Feedback?.Title,
                StylistId = booking.SalonMemberId,
                StylistName = booking.SalonMember?.User?.FullName ?? "Unknown Stylist",
                SalonName = booking.salon.salonName,
                Address = booking.salon.Address,
                ComboServiceName = booking.ComboService == null ? null : new ComboServiceForBookingDTO
                {
                    Id = booking.ComboServiceId,
                    ComboServiceName = booking.ComboService.ComboServiceName ?? "Unknown Service",
                    Price = booking.ComboService.Price,
                    Image = booking.ComboService.ImageUrl
                },
                PaymentAmount = booking.Payments?.PaymentAmount ?? 0,
                PaymentDate = booking.Payments?.PaymentDate ?? DateTime.MinValue,
                PaymentStatus = booking.Payments?.PaymentStatus?.StatusName
            };

            return new Result<object>
            {
                Error = 0,
                Message = "Booking Detail",
                Data = result
            };
        }

        public async Task<Result<object>> GetAllBookingWithAllStatus()
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllBookingWithAllStatus();

            if (bookings == null || !bookings.Any())
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Do not found any booking"
                };
            }

            var result = bookings.Select(booking => new BookingStatusDTO
            {
                BookingId = booking.Id,
                BookingDate = booking.BookingDate,
                BookingStatus = booking.BookingStatus,
                CustomerName = booking.CustomerName,
                CustomerPhoneNumber = booking.CustomerPhoneNumber,
                Feedback = booking.Feedback?.Title,
                StylistId = booking.SalonMemberId,
                StylistName = booking.SalonMember?.User?.FullName ?? "Unknown Stylist",
                Address = booking.salon.Address,
                ComboServiceName = booking.ComboService == null ? null : new ComboServiceForBookingDTO
                {
                    Id = booking.ComboServiceId,
                    ComboServiceName = booking.ComboService.ComboServiceName ?? "Unknown Service",
                    Price = booking.ComboService.Price,
                    Image = booking.ComboService.ImageUrl
                },
                PaymentAmount = booking.Payments?.PaymentAmount ?? 0,
                PaymentDate = booking.Payments?.PaymentDate ?? DateTime.MinValue,
                PaymentStatus = booking.Payments?.PaymentStatus?.StatusName
            }).ToList();

            return new Result<object>
            {
                Error = 0,
                Message = "All booking with all status",
                Data = result
            };
        }
    }
}
