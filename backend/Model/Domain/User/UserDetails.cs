using backend.Model.Domain.Chat;
using backend.Model.Domain.Follow;
using backend.Model.Domain.Notification;
using backend.Model.Domain.Post;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace backend.Model.Domain.User
{
    public class UserDetails : IdentityUser
    {
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount {  get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public List<PostFeed> Posts { get; set; } = new List<PostFeed>();
        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
        public ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();
        public ICollection<PostRetweet> PostRetweets { get; set; } = new List<PostRetweet>();
        public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
        public ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();
        public ICollection<Notify> Notifies { get; set; } = new List<Notify>();
        public ICollection<Notify> SentNotifies { get; set; } = new List<Notify>();
        public ICollection<UserConversation> UserConversations { get; set; } = new List<UserConversation>();
    } 
}
