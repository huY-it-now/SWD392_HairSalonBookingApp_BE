﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Abstracts.Combo
{
    public class AddComboServiceRequest
    {
        public string ComboServiceName { get; set; }
        public decimal Price { get; set; }
        public Guid SalonId { get; set; }
    }
}
