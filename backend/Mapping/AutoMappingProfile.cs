using AutoMapper;
using backend.Model.Domain.Post;
using backend.Model.Domain.User;
using backend.Model.Dtos.PostFeed;
using backend.Model.Dtos.User;

namespace backend.Mapping
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<UserDetails, UserResponseDto>().ReverseMap();
            CreateMap<UserDetails, UserRequestDto>().ReverseMap();
            CreateMap<PostFeed, PostFeedResponseDto>().ReverseMap();
            CreateMap<PostFeedRequestDto, PostFeed>().ReverseMap();
            CreateMap<PostLike,PostFeedRequestDto>().ReverseMap();
            CreateMap<PostLike,PostFeedResponseDto>().ReverseMap();
        }
    }
}
