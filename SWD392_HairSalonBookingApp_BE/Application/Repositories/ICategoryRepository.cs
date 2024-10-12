﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> GetAllCategory();
    }
}