namespace Domain.Contracts.DTO.Account
{
    public class RegisterUserDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
