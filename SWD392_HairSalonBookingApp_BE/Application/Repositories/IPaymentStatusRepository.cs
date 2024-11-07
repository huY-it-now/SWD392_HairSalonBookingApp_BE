using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IPaymentStatusRepository : IGenericRepository<PaymentStatus>
    {
        void Remove(PaymentStatus paymentStatus);
        Task<PaymentStatus> GetPaymentStatusByName (string name);
    }
}
