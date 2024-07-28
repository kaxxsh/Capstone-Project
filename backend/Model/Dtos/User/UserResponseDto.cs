using backend.Model.Dtos.User.UserFollow;

namespace backend.Model.Dtos.User
{
    public class UserResponseDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime JoinDate { get; set; }
        public string Gender { get; set; }
    }
}
