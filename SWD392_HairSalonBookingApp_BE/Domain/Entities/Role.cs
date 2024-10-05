using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string RoleDetail { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
