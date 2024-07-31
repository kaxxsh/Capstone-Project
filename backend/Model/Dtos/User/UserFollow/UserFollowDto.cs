namespace backend.Model.Dtos.User.UserFollow
{
    public class UserFollowDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public bool IsFollowed { get; set; }
        public string Bio { get; set; }
    }
}
