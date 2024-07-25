using AutoMapper;
using backend.Model.Domain.Follow;
using backend.Model.Domain.Post;
using backend.Model.Domain.User;
using backend.Model.Dtos.PostFeed;
using backend.Model.Dtos.PostFeed.CommentPost;
using backend.Model.Dtos.PostFeed.LikePost;
using backend.Model.Dtos.PostFeed.RetweetPost;
using backend.Model.Dtos.User;
using backend.Model.Dtos.User.UserFollow;

namespace backend.Mapping
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<UserDetails, UserResponseDto>().ReverseMap();
            CreateMap<UserDetails, UserRequestDto>().ReverseMap();
            CreateMap<PostFeed, PostFeedResponseDto>()
                .ForMember(dest => dest.PostLikes, opt => opt.MapFrom(src => src.PostLikes))
                .ForMember(dest => dest.PostComments, opt => opt.MapFrom(src => src.PostComments))
                .ForMember(dest => dest.PostRetweets, opt => opt.MapFrom(src => src.PostRetweets));
            CreateMap<PostFeedRequestDto, PostFeed>().ReverseMap();
            CreateMap<PostLike, LikePostResponseDto>().ReverseMap();
            CreateMap<PostComment, PostCommentResponseDto>()
                .ReverseMap();
            CreateMap<PostCommentRequestDto, PostComment>().ReverseMap();
            CreateMap<PostRetweet, PostRetweetResponseDto>()
                .ReverseMap();
            CreateMap<PostRetweetRequestDto, PostRetweet>().ReverseMap();
            CreateMap<PostRetweet, PostRetweetDto>()
                .ReverseMap();
            CreateMap<UserFollow, UserFollowDto>().ReverseMap();
        }
    }

}
