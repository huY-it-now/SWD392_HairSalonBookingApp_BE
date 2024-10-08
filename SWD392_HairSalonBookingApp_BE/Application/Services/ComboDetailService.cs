using Application.Repositories;
using Domain.Entities;

namespace Application.Services
{
    public class ComboDetailService
    {
        private readonly IComboDetailRepository _comboDetailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ComboDetailService(IComboDetailRepository comboDetailRepository, IUnitOfWork unitOfWork)
        {
            _comboDetailRepository = comboDetailRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ComboDetail> CreateComboDetailAsync(ComboDetail comboDetail)
        {
            await _comboDetailRepository.AddAsync(comboDetail);
            await _unitOfWork.SaveChangeAsync();
            return comboDetail;
        }

        public async Task<IEnumerable<ComboDetail>> GetAllComboDetailsAsync()
        {
            return await _comboDetailRepository.GetAllAsync();
        }

        public async Task<ComboDetail> GetComboDetailByIdAsync(Guid id)
        {
            return await _comboDetailRepository.GetByIdAsync(id);
        }

        public async Task UpdateComboDetailAsync(ComboDetail comboDetail)
        {
            _comboDetailRepository.Update(comboDetail);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task DeleteComboDetailAsync(Guid id)
        {
            var comboDetail = await _comboDetailRepository.GetByIdAsync(id);
            if (comboDetail != null)
            {
                _comboDetailRepository.SoftRemove(comboDetail);
                await _unitOfWork.SaveChangeAsync();
            }
        }
    }
}
