using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.BLL.Models;
using TodoList.DAL.Entities;

namespace TodoList.BLL.MappingProfiles
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<AddOrUpdateTaskVM, Todo>();
            // .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));
        }
    }

}
