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

        #region RelationShip
        public int RoleId { get; set; }  // Foreign key property
        public Role Role { get; set; }
        public Guid? SalonMemberId { get; set; }
        public SalonMember? SalonMember { get; set; }
        #endregion
    }
}
