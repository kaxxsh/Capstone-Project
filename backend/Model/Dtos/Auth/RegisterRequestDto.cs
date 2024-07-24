namespace backend.Model.Dtos.Auth
{
    public class RegisterRequestDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
