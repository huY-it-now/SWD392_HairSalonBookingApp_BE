using Application;
using Application.Interfaces;
using Application.Repositories;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;

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
        private readonly ISalonMemberRepository _salonMemberRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IComboServiceComboDetailRepository _comboServiceComboDetailRepository;
        private IAppointmentRepository _appointmentRepository;
        private IServiceComboServiceRepository _serviceComboServiceRepository;
        private IBookingRepository _bookingRepository;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IFeedbackRepository _feedbackRepository;

        public UnitOfWork(AppDbContext dbContext,
                          IUserRepository userRepository,
                          IComboServiceRepository comboServiceRepository,
                          IComboDetailRepository comboDetailRepository,
                          ICategoryRepository categoryRepository,
                          ISalonRepository salonRepository,
                          IServiceRepository serviceRepository,
                          ISalonMemberRepository salonMemberRepository,
                          IScheduleRepository scheduleRepository,
                          IComboServiceComboDetailRepository comboServiceComboDetailRepository,
                          IAppointmentRepository appointmentRepository, 
                          IServiceComboServiceRepository serviceComboServiceRepository, 
                          IBookingRepository bookingRepository,
                          ICurrentTime currentTime, 
                          IClaimsService claimsService, IFeedbackRepository feedbackRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _comboServiceRepository = comboServiceRepository;
            _comboDetailRepository = comboDetailRepository;
            _categoryRepository = categoryRepository;
            _serviceRepository = serviceRepository;
            _salonRepository = salonRepository;
            _salonMemberRepository = salonMemberRepository;
            _scheduleRepository = scheduleRepository;
            _comboServiceComboDetailRepository = comboServiceComboDetailRepository;
            _appointmentRepository = appointmentRepository;
            _serviceComboServiceRepository = serviceComboServiceRepository;
            _bookingRepository = bookingRepository;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _feedbackRepository = feedbackRepository;
        }
        
        public IUserRepository UserRepository => _userRepository;
        public IComboServiceRepository ComboServiceRepository => _comboServiceRepository;
        public IComboDetailRepository ComboDetailRepository => _comboDetailRepository;
        public ICategoryRepository CategoryRepository => _categoryRepository;
        public IServiceRepository ServiceRepository => _serviceRepository;
        public ISalonMemberRepository SalonMemberRepository => _salonMemberRepository;
        public ISalonRepository SalonRepository => _salonRepository;
        public IScheduleRepository ScheduleRepository => _scheduleRepository;
        public IComboServiceComboDetailRepository ComboServiceComboDetailRepository => _comboServiceComboDetailRepository;
        public IAppointmentRepository AppointmentRepository => _appointmentRepository;

        public IServiceComboServiceRepository ServiceComboServiceRepository => _serviceComboServiceRepository;

        public IBookingRepository BookingRepository => _bookingRepository;

        public IFeedbackRepository FeedbackRepository => _feedbackRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
