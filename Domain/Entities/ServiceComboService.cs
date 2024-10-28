namespace Domain.Entities
{
    public class ServiceComboService
    {
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
        public Guid ComboServiceId { get; set; }
        public ComboService ComboService { get; set; }
    }
}
