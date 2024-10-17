﻿using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Contracts.Abstracts.Category;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Category;
using Domain.Contracts.DTO.Service;
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
        public async Task<IActionResult> PrintAllCategory()
        {
            var category = await _categoryService.GetAllCategory();

            return Ok(category);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryById(id);

            return Ok(category);
        }

        [HttpPost("create-category")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest createRequest)
        {
            var categoryDTO = _mapper.Map<CreateCategoryDTO>(createRequest);
            var result = await _categoryService.CreateCategory(categoryDTO);

            return Ok(result);
        }
    }
}
