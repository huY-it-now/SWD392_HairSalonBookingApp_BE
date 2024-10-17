using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Abstracts.Shared;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<object>> GetAllCategory();
        Task<Result<object>> GetCategoryById(Guid id);
    }
}
