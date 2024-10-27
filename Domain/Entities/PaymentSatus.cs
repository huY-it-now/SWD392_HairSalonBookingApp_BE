﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PaymentSatus : BaseEntity
    {
        public string StatusName { get; set; } //Paid-Pending-Refunded
        public string Discription {  get; set; } 
        //Paid the amount
        //Waiting for pay
        //Have something wrong with booking so refund the money
        public ICollection<Payments> Payments { get; set; }
    }
}
