using Application;
using Application.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IComboServiceRepository _comboServiceRepository;
        private readonly IComboDetailRepository _comboDetailRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ISalonRepository _salonRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository, IComboServiceRepository comboServiceRepository, IComboDetailRepository comboDetailRepository, ICategoryRepository categoryRepository, ISalonRepository salonRepository, IServiceRepository serviceRepository, IScheduleRepository scheduleRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _comboServiceRepository = comboServiceRepository;
            _comboDetailRepository = comboDetailRepository;
            _categoryRepository = categoryRepository;
            _serviceRepository = serviceRepository;
            _salonRepository = salonRepository;
            _scheduleRepository = scheduleRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public IComboServiceRepository ComboServiceRepository => _comboServiceRepository;
        public IComboDetailRepository ComboDetailRepository => _comboDetailRepository;
        public ICategoryRepository CategoryRepository => _categoryRepository;
        public IServiceRepository ServiceRepository => _serviceRepository;
        public ISalonRepository SalonRepository => _salonRepository;
        public IScheduleRepository ScheduleRepository => _scheduleRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
