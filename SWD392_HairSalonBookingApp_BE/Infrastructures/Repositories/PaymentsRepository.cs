using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class PaymentsRepository : GenericRepository<Payments>, IPaymentsRepository
    {
        private readonly AppDbContext _dbContext;

        public PaymentsRepository(AppDbContext dbContext, 
                                    ICurrentTime timeService, 
                                    IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public void ChangeStatus(Payments payments)
        {
            throw new NotImplementedException();
        }

        public bool CheckPayment(Payments payments)
        {
            if (payments.PaymentStatus.StatusName.Equals("paid"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
