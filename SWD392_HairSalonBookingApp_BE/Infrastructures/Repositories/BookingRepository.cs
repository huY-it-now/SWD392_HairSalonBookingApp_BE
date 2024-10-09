﻿using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Infrastructures.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly AppDbContext _dbContext;

        public BookingRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Booking>> GetFullBookingInformation()
        {
            return await _dbContext.Bookings.Where(b => b.Checked == false).Include(b => b.BookingDetails).Include(b => b.SalonMembers).ToListAsync();
        }
    }
}