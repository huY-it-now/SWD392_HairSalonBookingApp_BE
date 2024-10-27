using Domain.Entities;

public class SalonMemberSchedule : BaseEntity
{
    public DateTime ScheduleDate { get; set; }
    public string WorkShifts { get; set; }
    #region RelationShip
    public Guid SalonMemberId { get; set; }
    public SalonMember SalonMember { get; set; }
    #endregion
}