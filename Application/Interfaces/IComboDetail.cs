using Domain.Contracts.Abstracts.Combo;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Combo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IComboDetail
    {
        Task<Result<object>> GetAllComboDetails();
        Task<Result<object>> GetComboDetailById(Guid id);
        Task<Result<object>> AddComboDetail(AddComboDetailRequest request);
        Task<Result<object>> UpdateComboDetail(UpdateComboDetailRequest request);
        Task<Result<object>> DeleteComboDetail(Guid id);
        //Task<Result<object>> GetComboDetailsByComboServiceId(Guid comboServiceId);

        // Thêm phương thức để lấy ComboService liên quan qua bảng trung gian
        Task<Result<object>> GetComboServicesByComboDetailId(Guid comboDetailId);
    }
}
