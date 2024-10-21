using Domain.Entities;

public class ViewSalonDTO {
    public Guid SalonId { get; set; }
    public List<ViewSalonMemberDTO> SalonMembers { get; set; }
}