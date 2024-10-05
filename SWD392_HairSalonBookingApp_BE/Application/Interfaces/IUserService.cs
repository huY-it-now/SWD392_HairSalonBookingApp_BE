﻿using Domain.Contracts.Abstracts.Account;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Account;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<object>> GetAllUser();
        Task<Result<object>> Register(RegisterUserDTO request);
    }
}
