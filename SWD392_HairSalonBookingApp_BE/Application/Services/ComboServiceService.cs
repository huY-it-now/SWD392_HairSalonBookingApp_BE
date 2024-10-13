using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;

namespace Application.Services
{
    public class ComboServiceService : IComboServiceService
    {
        private readonly IComboServiceRepository _comboServiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ComboServiceService(IComboServiceRepository comboServiceRepository, IUnitOfWork unitOfWork)
        {
            _comboServiceRepository = comboServiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ComboService> CreateComboServiceAsync(ComboService comboService)
        {
            await _comboServiceRepository.AddAsync(comboService);
            await _unitOfWork.SaveChangeAsync();
            return comboService;
        }

        public async Task<IEnumerable<ComboService>> GetAllComboServicesAsync()
        {
            return await _comboServiceRepository.GetAllAsync();
        }

        public async Task<ComboService> GetComboServiceByIdAsync(Guid id)
        {
            var comboService = await _comboServiceRepository.GetByIdAsync(id);

            if (comboService == null)
            {
                throw new KeyNotFoundException($"ComboService id {id} not found.");
            }

            return comboService;
        }


        public async Task UpdateComboServiceAsync(ComboService comboService)
        {
            _comboServiceRepository.Update(comboService);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task DeleteComboServiceAsync(Guid id)
        {
            var comboService = await _comboServiceRepository.GetByIdAsync(id);
            if (comboService != null)
            {
                _comboServiceRepository.SoftRemove(comboService);
                await _unitOfWork.SaveChangeAsync();
            }
        }
    }
}
