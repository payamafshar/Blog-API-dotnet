using AutoMapper;
using Blog_API.Modules.Blog;
using Blog_API.Modules.Users;

namespace Blog_API.Mapping
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()


        {
            //Excluding Password From Client
            CreateMap<BlogEntity, UsersEntity>().ForMember(dest => dest.UserName, opt => opt.Ignore());
        }
    }
}
