namespace Domain.Entities
{
    public class SalonWithMembers
    {
        public Guid SalonId { get; set; }
        public Salon Salon { get; set; }
        public Guid SalonMemberId { get; set; }
        public SalonMember SalonMember { get; set; }
    }
}
