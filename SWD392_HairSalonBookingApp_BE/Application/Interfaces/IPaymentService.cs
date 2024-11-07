using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> BookingPaymentCheck(Guid BookingId); 
        Task<Payments> GetPaymentsById(Guid Id);
        Task<bool> ChangePaymentStatus(Guid bookingId, string status);
        Task<bool> CreatePayment();
        Task<bool> CreatePaymentStatus(string StatusName, string Description);
        Task<bool> CreatePaymentMethod(string MethodName, string Description);
        Task<bool> UpdatePaymentStatus(Guid PaymentStatusId, string StatusName, string Description);
        Task<bool> DeletePaymentStatus(Guid PaymentStatusId);
        Task<bool> UpdatePayment(Guid PaymentId, string PaymentStatus);
    }
}
