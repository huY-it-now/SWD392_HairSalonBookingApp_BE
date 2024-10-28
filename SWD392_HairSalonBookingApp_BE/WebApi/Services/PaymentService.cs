using Application;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Infrastructures;
using Infrastructures.Repositories;
using System.Reflection;

namespace WebApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IPaymentLogRepository _paymentLogRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IPaymentStatusRepository _paymentStatusRepository;

        public PaymentService(IPaymentsRepository paymentsRepository, IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IPaymentLogRepository paymentLogRepository, IPaymentMethodRepository paymentMethodRepository, IPaymentStatusRepository paymentStatusRepository)
        {
            _paymentLogRepository = paymentLogRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _paymentStatusRepository = paymentStatusRepository;
            _unitOfWork = unitOfWork;
            _bookingRepository = bookingRepository;
            _paymentsRepository = paymentsRepository;
        }
        public async Task<bool> BookingPaymentCheck(Guid bookingId)
        {
            var booking = await _bookingRepository.GetBookingWithPayment(bookingId);

            if (booking == null)
            {
                return false;
            }

            return _paymentsRepository.CheckPayment(booking.Payments);
        }

        public async Task<bool> ChangePaymentStatus(Guid bookingId, string status)
        {
            var booking = await _bookingRepository.GetBookingWithPayment(bookingId);

            if (booking == null)
            {
                return false;
            }

            var payment = await _paymentsRepository.GetByIdAsync(booking.PaymentId);

            if (payment != null)
            {
                payment.PaymentStatus.StatusName = status;

                _paymentsRepository.Update(payment);

                return await _unitOfWork.SaveChangeAsync() > 0;
            }
            else
            {
                return false;
            }
        }

        public Task<bool> CreatePayment()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreatePaymentMethod(string MethodName, string Description)
        {
            PaymentMethods paymentMethods = new();
            paymentMethods.MethodName = MethodName;
            paymentMethods.Description = Description;

            await _paymentMethodRepository.AddAsync(paymentMethods);

            if (!(await _unitOfWork.SaveChangeAsync() > 0))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CreatePaymentStatus(string StatusName, string Description)
        {
            PaymentStatus paymentStatus = new();
            paymentStatus.StatusName = StatusName;
            paymentStatus.Description = Description;

            await _paymentStatusRepository.AddAsync(paymentStatus);

            if (!(await _unitOfWork.SaveChangeAsync() > 0))
            {
                return false;
            }
            return true;
        }

        public Task<Payments> GetPaymentsById(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
