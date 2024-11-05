using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Abstracts.Category;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Category;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<object>> GetAllCategory();

        Task<Result<object>> GetCategoryById(Guid id);

        Task<Result<object>> CreateCategory(CreateCategoryDTO request);

        Task<Result<object>> UpdateCategory(Guid id, UpdateCategoryDTO request);

        Task<Result<object>> DeleteCategory(Guid id);

    }
}
