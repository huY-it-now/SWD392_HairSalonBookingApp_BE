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
        private readonly IBookingService _bookingService;

        public AdminController(IUserService userService, IMapper mapper, ISalonService salonService, IBookingService bookingService)
        {
            _userService = userService;
            _mapper = mapper;
            _salonService = salonService;
            _bookingService = bookingService;
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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> PrintAllSalonMember()
        {
            var result = await _userService.PrintAllSalonMember();
            return Ok(result);
        }

        [HttpPost("get-member-with-role")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetMemberWithRole(int roleId)
        {
            var result = await _userService.GetSalonMemberWithRole(roleId);
            return Ok(result);
        }

        [HttpGet("admin-dashboard")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetAdminDashboard()
        {
             var result = await _userService.GetAdminDashboard();
             return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllStaff()
        {
            var result = await _userService.GetAllStaff();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllManager()
        {
            var result = await _userService.GetAllManager();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(Guid userId)
        {
            var result = await _userService.BanUser(userId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> ViewListFeedback()
        {
            var result = await _userService.GetListFeedback();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllCustomer()
        {
            var result = await _userService.GetAllCustomer();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> CountCustomer()
        {
            var result = await _userService.CountCustomer();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UnbanUser(Guid userId)
        {
            var result = await _userService.UnBanUser(userId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> BanSalonMember(Guid salonMemberId)
        {
            var result = await _userService.BanSalonMember(salonMemberId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UnbanSalonMember(Guid salonMemberId)
        {
            var result = await _userService.UnbanSalonMember(salonMemberId);
            return Ok(result);
        }
    }
}
