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
    }
}
