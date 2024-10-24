using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Contracts.Abstracts.Shared;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace WebApi.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ISalonService _salonService;

        public AdminController(IUserService userService, IMapper mapper, ISalonService salonService)
        {
            _userService = userService;
            _mapper = mapper;
            _salonService = salonService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> PrintAllUser()
        {
            var users = await _userService.GetAllUser();

            return Ok(users);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> PrintAllSalon()
        {
            var salon = await _salonService.PrintAllSalon();
            return Ok(salon);
        }
    }
}
