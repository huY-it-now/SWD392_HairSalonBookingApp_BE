using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? Address { get; set; }

        #region RelationShip
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public Guid? SalonMemberId { get; set; }
        public SalonMember? SalonMember { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        #endregion
    }
}
