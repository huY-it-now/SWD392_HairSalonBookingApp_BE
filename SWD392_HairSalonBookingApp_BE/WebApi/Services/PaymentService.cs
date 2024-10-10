using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Infrastructures.Repositories;

namespace WebApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentsRepository _paymentsRepository;

        public PaymentService(IPaymentsRepository paymentsRepository, IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
            _paymentsRepository = paymentsRepository;
        }
        public async Task<bool> BookingPaymentCheck(Guid BookingId)
        {
            var booking = await _bookingRepository.GetBookingWithPayment(BookingId);

            if (booking == null)
            {
                return false;
            }

            return _paymentsRepository.CheckPayment(booking.Payments);
        }
    }
}
