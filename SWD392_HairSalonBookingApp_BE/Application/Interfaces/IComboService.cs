using Domain.Contracts.Abstracts.Combo;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Combo;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IComboService
    {
        Task<Result<object>> GetAllComboServices();
        Task<Result<object>> GetAllComboDetailByComboServiceId(Guid id);
        Task<Result<object>> AddComboService(AddComboServiceRequest request);
        Task<Result<object>> UpdateComboService(UpdateComboServiceRequest request);
        Task<Result<object>> DeleteComboService(Guid id);
        Task<Result<object>> GetComboDetailsByComboServiceId(Guid comboServiceId);
        Task<Result<object>> AddComboDetailIntoComboService(Guid comboServiceId, Guid comboDetailId);
    }
}
