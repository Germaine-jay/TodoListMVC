using TodoList.BLL.Models;
using TodoList.DAL.Entities;
using AutoMapper;


namespace TodoList.BLL.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserVM>();
            CreateMap<UserVM, User>();

        }
    }
}
