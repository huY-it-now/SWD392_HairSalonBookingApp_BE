using Domain.Entities;

public class SalonMemberSchedule : BaseEntity
{
    public DateTime ScheduleDate { get; set; }
    public bool IsDayOff { get; set; }
    public bool IsFullDay { get; set; }
    public List<string> WorkShifts { get; set; }

    #region RelationShip
    public Guid SalonMemberId { get; set; }
    public SalonMember SalonMember { get; set; }
    #endregion
}