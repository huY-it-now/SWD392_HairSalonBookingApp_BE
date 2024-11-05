using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain.Contracts.Abstracts.Category;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Category;
using Domain.Contracts.DTO.Service;
using Domain.Entities;
using MimeKit.Cryptography;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IMapper mapper, 
                               IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<object>> GetAllCategory()
        {
            var categories = await _unitOfWork
                                    .CategoryRepository
                                    .GetAllCategoryAsync();

            var categoryDTOList = categories.Select(category => new CategoryDTO
            {
                Id = category.Id,
                CategoryName = category.CategoryName,

                Services = category.Services.Select(service => new ServiceDTO
                {
                    Id = service.Id,
                    ServiceName = service.ServiceName
                }).ToList()

            }).ToList();

            return new Result<object>
            {
                Error = 0,
                Message = "Print all categories with services",
                Data = categoryDTOList
            };
        }

        public async Task<Result<object>> GetCategoryById(Guid id)
        {
            var category = await _unitOfWork
                                    .CategoryRepository
                                    .GetCategoryById(id);

            if (category == null || category.IsDeleted)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Category not found or has been deleted!",
                    Data = null
                };
            }

            var categoryDTO = new CategoryDTO
            {
                Id = category.Id,
                CategoryName = category.CategoryName,

                Services = category.Services.Select(s => new ServiceDTO
                {
                    Id = s.Id,
                    ServiceName = s.ServiceName
                }).ToList()
            };

            return new Result<object>
            {
                Error = 0,
                Message = "Category details etrieved successfully!",
                Data = categoryDTO
            };
        }

        public async Task<Result<object>> CreateCategory(CreateCategoryDTO createRequest)
        {
            var cId = createRequest.CategoryId = Guid.NewGuid();

            var category = new Category
            {
                Id = cId,
                CategoryName = createRequest.CategoryName,
                IsDeleted = false
            };

            var createdCategory = await _unitOfWork
                                            .CategoryRepository
                                            .CreateCategory(category);

            if (createdCategory == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Category name already exists.",
                    Data = null
                };
            }

            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "An error occurred while creating the category.",
                    Data = null
                };
            }

            var result = _mapper.Map<CategoryDTO>(category);

            return new Result<object>
            {
                Error = 0,
                Message = "Category created successully!",
                Data = result
            };


        }

        public async Task<Result<object>> UpdateCategory(Guid id, UpdateCategoryDTO updateRequest)
        {
            var category = await _unitOfWork
                                    .CategoryRepository
                                    .GetCategoryById(id);

            if (category == null || category.IsDeleted)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Category not found or has been deleted!",
                    Data = null
                };
            }

            category.CategoryName = updateRequest.CategoryName;

            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "An error occurred while updating the category.",
                    Data = null
                };
            }

            var result = _mapper.Map<CategoryDTO>(category);

            return new Result<object>
            {
                Error = 0,
                Message = "Category updated successfully!",
                Data = result
            };
        }

        public async Task<Result<object>> DeleteCategory(Guid id)
        {
            var category = await _unitOfWork
                                    .CategoryRepository
                                    .GetCategoryById(id);

            if (category == null || category.IsDeleted)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Category not found or has already been deleted!",
                    Data = null
                };
            }

            category.IsDeleted = true;

            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Category marked as deleted successfully!",
                Data = null
            };
        }
    }
}
