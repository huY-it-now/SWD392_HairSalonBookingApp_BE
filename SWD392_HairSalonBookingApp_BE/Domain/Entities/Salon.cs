using Microsoft.AspNetCore.Http;

namespace Domain.Entities
{
    public class Salon : BaseEntity
    {
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string? ImageId { get; set; }

        #region RelationShip
        public ICollection<ComboService> ComboServices { get; set; }
        public ICollection<SalonMember> SalonMembers { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; }
        #endregion
    }
}
