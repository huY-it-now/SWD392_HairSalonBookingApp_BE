using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Category;
using Domain.Contracts.DTO.Service;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<object>> GetAllCategory()
        {
            var category = await _unitOfWork.CategoryRepository.GetAllCategoryAsync();
            var categoryMapper = _mapper.Map<List<CategoryDTO>>(category);

            return new Result<object>
            {
                Error = 0,
                Message = "Print all category",
                Data = categoryMapper
            };
        }

        public async Task<Result<object>> GetCategoryById(Guid id)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryById(id);

            if (category == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Category not found!",
                    Data = null
                };
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(category);

            return new Result<object>
            {
                Error = 0,
                Message = "Category details etrieved successfully!",
                Data = categoryDTO
            };
        }
    }
}
