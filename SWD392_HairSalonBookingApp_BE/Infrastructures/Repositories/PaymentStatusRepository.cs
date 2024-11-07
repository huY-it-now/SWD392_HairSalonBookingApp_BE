using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class PaymentStatusRepository : GenericRepository<PaymentStatus>, IPaymentStatusRepository
    {
        private readonly AppDbContext _dbContext;

        public PaymentStatusRepository(AppDbContext dbContext, 
                                       ICurrentTime timeService, 
                                       IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<PaymentStatus> GetPaymentStatusByName(string name)
        {
            return await _dbContext.PaymentSatus.Where(ps => ps.StatusName == name).FirstOrDefaultAsync();
        }

        public void Remove(PaymentStatus paymentStatus)
        {
            _dbContext.Remove(paymentStatus);
        }
    }
}
