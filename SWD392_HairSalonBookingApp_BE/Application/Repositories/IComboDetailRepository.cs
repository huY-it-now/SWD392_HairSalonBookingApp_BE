using Domain.Entities;

namespace Application.Repositories
{
    public interface IComboDetailRepository : IGenericRepository<ComboDetail>
    {
        Task<List<ComboDetail>> GetAllComboDetailsAsync();
        Task<ComboDetail> GetComboDetailById(Guid id);
        Task<ComboDetail> AddComboDetail(ComboDetail comboDetail);
        Task<ComboDetail> UpdateComboDetail(ComboDetail comboDetail);
        Task DeleteComboDetail(Guid id);

        // Thêm phương thức để lấy ComboService liên quan
        Task<List<ComboService>> GetComboServicesByComboDetailId(Guid comboDetailId);
        Task<ComboDetail> CheckComboDetailExistByName(string name);
        //Task<List<ComboDetail>> GetComboDetailsByComboServiceId(Guid comboServiceId);
    }
}
