using Application.Interfaces;
using Domain.Entities;
using Infrastructures;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;

public class SalonMemberRepository : GenericRepository<SalonMember>, ISalonMemberRepository {
    private readonly AppDbContext _dbContext;

    public SalonMemberRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService) {
        _dbContext = dbContext;
    }

    public async Task<List<SalonMember>> GetAllSalonMember() {
        return await _dbContext.SalonMembers.Include(x => x.User).ToListAsync();
    }

    public async Task<List<SalonMember>> GetSalonMemberWithRole(int roleId) {
        return await _dbContext.SalonMembers.Include(x => x.User).Where(x => x.User.RoleId == roleId).ToListAsync();
    }

    public async Task<List<SalonMember>> GetSalonMemberBySalonId(Guid salonId) {
        return await _dbContext.SalonMembers.Where(x => x.SalonId == salonId)
            .Include(x => x.User).Include(x => x.Salon).ToListAsync();
    }

    public async Task<List<SalonMember>> GetSalonMembersFree(DateTime dateTime, Salon salon, int HourStart, int HourEnd, int minuteStart, int minutEnd, SalonMember salonMember)
    {
        var listStylist = await _dbContext.SalonMembers.Include(st => st.SalonMemberSchedules).Where(st => st.Salon == salon).ToListAsync();

        if (listStylist.Count == 0)
        {
            return null;
        }

        if (salonMember != null)
        {
            listStylist.Remove(salonMember);
        }

        List<SalonMemberSchedule> listschedules = new();

        foreach (var item in listStylist)
        {
            if (item.SalonMemberSchedules != null)
            {
                foreach (var schedule in item.SalonMemberSchedules)
                {
                    if (schedule.ScheduleDate.Day == dateTime.Day && schedule.ScheduleDate.Month == dateTime.Month && schedule.ScheduleDate.Year == dateTime.Year)
                    {
                        listschedules.Add(schedule);
                    }
                }
            }
        }

        foreach (var item in listschedules)
        {
            if (item.WorkTime != null)
            {
                foreach (var workTime in item.WorkTime)
                {
                    if (workTime.ScheduleDate.Hour > HourStart && workTime.ScheduleDate.Hour < HourEnd)
                    {
                        foreach (var sty in listStylist)
                        {
                            if (sty.SalonMemberSchedules == item)
                            {
                                listStylist.Remove(sty);
                                break;
                            }
                        }
                    }
                }
            }
        }


        return listStylist;
    }

    public async Task<List<SalonMember>> GetStylistBySalonId(Guid salonId)
    {
        return await _dbContext.SalonMembers.Where(x => x.SalonId == salonId).Include(x => x.User).Where(x => x.User.RoleId == 5).ToListAsync();
    }

    public async Task<SalonMember> GetSalonMemberById(Guid stylistId)
    {
        return await _dbContext.SalonMembers.Where(x => x.Id == stylistId).FirstOrDefaultAsync();
    }
}