using Application;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Infrastructures;
using Infrastructures.Repositories;

namespace WebApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentsRepository _paymentsRepository;

        public PaymentService(IPaymentsRepository paymentsRepository, IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
        {
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
                payment.PaymentSatus.StatusName = status;

                _paymentsRepository.Update(payment);

                return await _unitOfWork.SaveChangeAsync() > 0;
            }
            else
            {
                return false;
            }
        }

        public Task<Payments> GetPaymentsById(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
