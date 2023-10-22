using AutoMapper;
using Blog_API.Identity;
using Blog_API.Modules.Blog;
using Blog_API.Modules.Blog.Dtos;
using Blog_API.Modules.Users;
using Blog_API.Modules.Users.Dtos;

namespace Blog_API.Mapping
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()


        {
            //Excluding Password From Client
            CreateMap<BlogEntity, UsersEntity>().ForMember(dest => dest.UserName, opt => opt.Ignore());
            CreateMap<RegisterDto, ApplicationUser>().ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            //--------
            CreateMap<BlogEntity, CreateBlogDto>().ReverseMap();
        }
    }
}
