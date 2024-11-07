using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IPaymentsRepository : IGenericRepository<Payments>
    {
        public bool CheckPayment(Payments payments);
        public void ChangeStatus(Payments payments);
        public Task<Payments> GetPaymentById (Guid paymentId);
    }
}
