using AutoMapper;
using backend.Model.Domain.Chat;
using backend.Model.Domain.Follow;
using backend.Model.Domain.Notification;
using backend.Model.Domain.Post;
using backend.Model.Domain.User;
using backend.Model.Dtos.Chat;
using backend.Model.Dtos.Notify;
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
            CreateMap<UserDetails, UserDto>().ReverseMap();

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

            CreateMap<Notify, NotifyRequestDto>().ReverseMap();
            CreateMap<Notify, NotifyResponseDto>()
                .ForMember(dest => dest.FromUserName, opt => opt.MapFrom(src => src.FromUser.UserName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Sender.Name))
                .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.MessageId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.SentAt));
            CreateMap<Conversation, ConversationDto>()
                .ForMember(dest => dest.User1Name, opt => opt.MapFrom(src => src.UserConversations.FirstOrDefault().User.Name))
                .ForMember(dest => dest.User2Name, opt => opt.MapFrom(src => src.UserConversations.Skip(1).FirstOrDefault().User.Name))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages));
            CreateMap<UserConversation, ConversationDto>()
                .ForMember(dest => dest.ConversationId, opt => opt.MapFrom(src => src.ConversationId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Conversation.CreatedAt))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Conversation.Messages));

            CreateMap<Conversation, ConversationUserDto>()
                .ForMember(dest => dest.ConversationId, opt => opt.MapFrom(src => src.ConversationId))
                .ForMember(dest => dest.User1Name, opt => opt.MapFrom(src => src.UserConversations.FirstOrDefault().User.Name))
                .ForMember(dest => dest.User2Name, opt => opt.MapFrom(src => src.UserConversations.Skip(1).FirstOrDefault().User.Name));
        }
    }

}
