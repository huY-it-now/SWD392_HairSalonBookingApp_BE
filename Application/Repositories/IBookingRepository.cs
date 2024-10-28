﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<Booking> GetBookingWithPayment(Guid id); 
        Task<List<Booking>> GetUncheckBookingInformation();
        Task<List<Booking>> GetCheckedBookingInformation();
    }
}
