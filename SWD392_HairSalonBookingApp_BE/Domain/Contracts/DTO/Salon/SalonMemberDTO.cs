namespace Domain.Contracts.DTO.Salon
{
    public class SalonMemberDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string SalonName { get; set; }
    }
}
