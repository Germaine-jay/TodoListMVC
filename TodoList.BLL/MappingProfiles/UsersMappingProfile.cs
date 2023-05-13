
using TodoList.BLL.Models;
using TodoList.DAL.Entities;
using AutoMapper;


namespace TodoList.BLL.MappingProfiles
{
    public class UsersMappingProfile: Profile
    {
        public UsersMappingProfile()
        {      
            CreateMap<DeleteUserVM, User>();
            CreateMap<User, DeleteUserVM>();
        }
    }
}
