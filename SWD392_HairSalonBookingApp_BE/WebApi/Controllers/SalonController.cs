﻿using Application.Interfaces;
using Application.Validations.Salon;
using AutoMapper;
using Domain.Contracts.Abstracts.Salon;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Salon;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace WebApi.Controllers
{
    public class SalonController : BaseController
    {
        private readonly ISalonService _salonService;
        private readonly IMapper _mapper;

        public SalonController(ISalonService salonService, IMapper mapper)
        {
            _salonService = salonService;
            _mapper = mapper;
        }

        [HttpPost("CreateSalon")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> CreateSalon(CreateSalonRequest request)
        {
            var validator = new CreateSalonRequestValidation();
            var validatorResult = validator.Validate(request);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Validation failed!",
                    Data = validatorResult.Errors.Select(x => x.ErrorMessage).ToList(),
                });
            }

            var salonMapper = _mapper.Map<CreateSalonDTO>(request);
            var result = await _salonService.CreateSalon(salonMapper);
            return Ok(result);
        }

        [HttpPost("search-salon-address")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> SearchSalonWithAddress(SearchSalonRequest request) {
            var salonMapper = _mapper.Map<SalonDTO>(request);
            var result = await _salonService.SearchSalonWithAddress(salonMapper);

            return Ok(result);
        }

        [HttpPost("search-salon-id")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> SearchSalonWithId(SearchSalonWithIdRequest request) {
            var salonMapper = _mapper.Map<SalonDTO>(request);
            var result = await _salonService.SearchSalonById(salonMapper);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewSalonMemberBySalonId([FromForm] ViewSalonRequest request) {
            var validator = new ViewSalonRequestValidator();
            var validatorResult = validator.Validate(request);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Validation failed!",
                    Data = validatorResult.Errors.Select(x => x.ErrorMessage).ToList(),
                });
            }

            var mapper = _mapper.Map<ViewSalonDTO>(request);
            var result = await _salonService.ViewSalonMemberBySalonId(mapper);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> ViewStylistBySalonId(Guid salonId)
        {
            var result = await _salonService.ViewStylistBySalonId(salonId);

            return Ok(result);
        }
    }
}
