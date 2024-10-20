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
        Task<Result<object>> GetComboServiceById(Guid id);
        Task<Result<object>> AddComboService(AddComboServiceRequest request);
        Task<Result<object>> UpdateComboService(UpdateComboServiceRequest request);
        Task<Result<object>> DeleteComboService(Guid id);

        // Thêm phương thức để lấy ComboDetail liên quan qua bảng trung gian
        Task<Result<object>> GetComboDetailsByComboServiceId(Guid comboServiceId);
    }
}
