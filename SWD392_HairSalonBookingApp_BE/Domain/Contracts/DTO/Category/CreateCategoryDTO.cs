﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Category
{
    public class CreateCategoryDTO
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
