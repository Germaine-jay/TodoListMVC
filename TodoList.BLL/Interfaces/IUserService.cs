using System;
using System.Collections.Generic;
using TodoList.BLL.Models;
using TodoList.DAL.Entities;

namespace TodoList.BLL.Interfaces
{
    public interface IUserService
    {
        Task<(bool successful, string msg)>  Create(CreateUserVM model);
        Task<(bool successful, string msg)> Update(UserVM model);

        Task<(bool successful, string msg)> DeleteAsync(int userId);
        Task<IEnumerable<CreateUserVM>> GetUsers();
        Task<CreateUserVM> GetUser(int userId);
        Task<IEnumerable<UserWithTaskVM>> GetUsersWithTasksAsync();
        Task<(bool successful, string msg)> AddOrUpdateAsync(CreateUserVM model);
    }

}
