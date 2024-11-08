﻿using Domain.Contracts.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Category
{
    public class CategoryDTO
    {
        public string CategoryName { get; set; }
        public Guid Id { get; set; }
        public List<ServiceDTO> Services { get; set; }
    }
}
