using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Contracts.Abstracts.Shared;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace WebApi.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        [ProducesResponseType(500, Type = typeof(Result<object>))]
        public async Task<IActionResult> PrintAllCategory()
        {
            var category = await _categoryService.GetAllCategory();
            try
            {
                var result = await _categoryService.GetAllCategory();

                if (result.Data == null)
                {
                    return BadRequest(new Result<object>
                    {
                        Error = 1,
                        Message = "No categories found!",
                        Data = null
                    });
                }

                return Ok(new Result<object>
                {
                    Error = 0,
                    Message = "Category list retrieved successfully!",
                    Data = result.Data
                });
            }
            catch 
            {
                return StatusCode(500, new Result<object>
                {
                    Error = 500,
                    Message = "An error occurred while retrieving the categories!",
                    Data = null
                });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        [ProducesResponseType(500, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            try
            {
                var result = await _categoryService.GetCategoryById(id);

                if (result.Data == null)
                {
                    return BadRequest(new Result<object>
                    {
                        Error = 1,
                        Message = "Category not found!",
                        Data = null
                    });
                }

                return Ok(new Result<object>
                {
                    Error = 0,
                    Message = "Category retrieved successfully!",
                    Data = result.Data
                });
            }
            catch
            {
                return StatusCode(500, new Result<object>
                {
                    Error = 500,
                    Message = "An error occurred while retrieving the category!",
                    Data = null
                });
            }
        }

    }
}
