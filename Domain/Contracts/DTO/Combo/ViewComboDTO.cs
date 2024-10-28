namespace Domain.Contracts.DTO.Combo
{
    public class ViewComboDTO
    {
        public Guid ComboServiceId { get; set; }
        public string ComboServiceName { get; set; }
        public string Image { get; set; }
        public Guid ComboDetailId { get; set; }
        public string Content { get; set; }
    }
}
