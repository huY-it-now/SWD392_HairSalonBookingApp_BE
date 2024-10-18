﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PaymentMethods : BaseEntity
    {
        public string MethodName { get; set; }
        public string Description { get; set; }
        public Guid PaymentMethodId { get; set; }
        public ICollection<Payments> Payments { get; set; }
    }
}
