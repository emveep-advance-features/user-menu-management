using AutoMapper;
using role_management_user.Dtos;
using role_management_user.Models;

namespace role_management_user.Profiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Menu, MenuReadDto>();
            // CreateMap<User, UserReadDto>();
            CreateMap<MenuCreateDto, Menu>();
            CreateMap<MenuUpdateDto, Menu>();
        }
    }
}