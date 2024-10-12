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

        public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository, IComboServiceRepository comboServiceRepository, IComboDetailRepository comboDetailRepository, ICategoryRepository categoryRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _comboServiceRepository = comboServiceRepository;
            _comboDetailRepository = comboDetailRepository;
            _categoryRepository = categoryRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public IComboServiceRepository ComboServiceRepository => _comboServiceRepository;
        public IComboDetailRepository ComboDetailRepository => _comboDetailRepository;

        public ICategoryRepository CategoryRepository => _categoryRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
