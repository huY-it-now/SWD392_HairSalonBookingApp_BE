using Application.Interfaces;
using Domain.Entities;
using Infrastructures;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;

public class SalonMemberRepository : GenericRepository<SalonMember>, ISalonMemberRepository {
    private readonly AppDbContext _dbContext;

    public SalonMemberRepository(AppDbContext dbContext, 
                                 ICurrentTime timeService, 
                                 IClaimsService claimsService) : base(dbContext, timeService, claimsService) {
        _dbContext = dbContext;
    }

    public async Task<List<SalonMember>> GetAllSalonMember() {
        return await _dbContext.SalonMembers
                                    .Include(x => x.User)
                                    .ToListAsync();
    }

    public async Task<List<SalonMember>> GetSalonMemberWithRole(int roleId) {
        return await _dbContext.SalonMembers
                                    .Include(x => x.User)
                                    .Where(x => x.User.RoleId == roleId)
                                    .ToListAsync();
    }

    public async Task<List<SalonMember>> GetSalonMemberBySalonId(Guid salonId) {
        return await _dbContext.SalonMembers
                                    .Where(x => x.SalonId == salonId)
                                    .Include(x => x.User)
                                    .Include(x => x.Salon)
                                    .ToListAsync();
    }

    public async Task<List<SalonMember>> GetSalonMembersFree(DateTime dateTime, Guid salonId, int HourStart, int HourEnd, int minuteStart, int minutEnd, SalonMember salonMember)
    {
        var listStylist = await _dbContext.SalonMembers
        .Include(x => x.User)
        .Where(x => x.User.RoleId == 5)
        .Include(x => x.Salon)
        .Where(x => x.SalonId == salonId)
        .Include(st => st.SalonMemberSchedules)
        .ToListAsync();

        if (listStylist.Count == 0)
        {
            return null;
        }

        List<SalonMember> availableStylists = new();

        foreach (var stylist in listStylist)
        {
            bool isAvailable = true;

            if (stylist.SalonMemberSchedules != null)
            {
                foreach (var schedule in stylist.SalonMemberSchedules)
                {
                    if (schedule.ScheduleDate.Date == dateTime.Date)
                    {
                        if (schedule.WorkShifts != null && schedule.WorkShifts.Count > 0)
                        {
                            foreach (var shift in schedule.WorkShifts)
                            {
                                var times = shift.Split('-');
                                if (times.Length == 2)
                                {
                                    var workStart = DateTime.ParseExact(times[0], "HH:mm", null);
                                    var workEnd = DateTime.ParseExact(times[1], "HH:mm", null);

                                    workStart = new DateTime(schedule.ScheduleDate.Year, schedule.ScheduleDate.Month, schedule.ScheduleDate.Day, workStart.Hour, workStart.Minute, 0);
                                    workEnd = new DateTime(schedule.ScheduleDate.Year, schedule.ScheduleDate.Month, schedule.ScheduleDate.Day, workEnd.Hour, workEnd.Minute, 0);

                                    if (dateTime >= workStart && dateTime < workEnd)
                                    {
                                        isAvailable = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (!isAvailable)
                    {
                        break;
                    }
                }
            }

            if (isAvailable)
            {
                availableStylists.Add(stylist);
            }
        }

        Random random = new Random();
        availableStylists = availableStylists.OrderBy(x => random.Next()).ToList();

        return availableStylists;
    }

    public async Task<List<SalonMember>> GetStylistBySalonId(Guid salonId)
    {
        return await _dbContext.SalonMembers
                                    .Where(x => x.SalonId == salonId)
                                    .Include(x => x.User)
                                    .Where(x => x.User.RoleId == 5)
                                    .ToListAsync();
    }

    public async Task<SalonMember> GetSalonMemberById(Guid stylistId)
    {
        return await _dbContext.SalonMembers.Include(x => x.User).Where(x => x.Id == stylistId).FirstOrDefaultAsync();
    }
}