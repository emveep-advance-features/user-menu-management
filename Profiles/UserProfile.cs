using AutoMapper;
using role_management_user.Dtos;
using role_management_user.Models;

namespace role_management_user.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<RegisterModel, User>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}