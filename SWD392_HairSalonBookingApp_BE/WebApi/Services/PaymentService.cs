﻿using Application;
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

        public async Task<bool> DeletePaymentStatus(Guid PaymentStatusId)
        {
            var paymentStatus = await _paymentStatusRepository.GetByIdAsync(PaymentStatusId);

            if (paymentStatus == null)
            {
                return false;
            }

            _paymentStatusRepository.Remove(paymentStatus);

            return (await _unitOfWork.SaveChangeAsync() > 0);
        }

        public async Task<Payments> GetPaymentsById(Guid Id)
        {
            return await _paymentsRepository.GetByIdAsync(Id);
        }

        public async Task<bool> UpdatePayment(Guid PaymentId, string PaymentStatus)
        {
            var payment = await _paymentsRepository.GetPaymentById(PaymentId);

            if (payment == null)
            {
                return false;
            }

            var Description = GetPaymentStatusDescription(PaymentStatus);

            if (string.IsNullOrEmpty(Description))
            {
                return false;
            }

            payment.PaymentStatus.StatusName = PaymentStatus;
            payment.PaymentStatus.Description = Description;

            _paymentsRepository.Update(payment);

            return (await _unitOfWork.SaveChangeAsync() > 0);
        }

        public string GetPaymentStatusDescription (string PaymentStatus)
        {
            if (PaymentStatus == "Cancel")
            {
                return "Payment is cancel.";
            }
            else if (PaymentStatus == "Pending")
            {
                return "Payment is waiting for pay.";
            }
            else if (PaymentStatus == "Paid")
            {
                return "Payment is completed.";
            }
            else
            {
                return "";
            }
        }

        public async Task<bool> UpdatePaymentStatus(Guid PaymentStatusId, string StatusName, string Description)
        {
            var paymentStatus = await _paymentStatusRepository.GetByIdAsync(PaymentStatusId);

            if (paymentStatus == null)
            {
                return false;
            }

            paymentStatus.StatusName = StatusName;
            paymentStatus.Description = Description;

            _paymentStatusRepository.Update(paymentStatus);

            return (await _unitOfWork.SaveChangeAsync() > 0);
        }
    }
}
