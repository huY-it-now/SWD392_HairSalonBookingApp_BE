namespace Domain.Contracts.DTO.Combo
{
    public class ComboServiceForBookingDTO
    {
        public Guid Id { get; set; }
        public string ComboServiceName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
