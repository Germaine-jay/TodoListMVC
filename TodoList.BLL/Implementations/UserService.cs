using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoList.BLL.Interfaces;
using TodoList.BLL.Models;
using TodoList.DAL.Entities;
using TodoList.DAL.Repository;

namespace TodoList.BLL.Implementations
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepo;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepo = _unitOfWork.GetRepository<User>();
        }


        public async Task<(bool successful, string msg)> Create(DeleteUserVM model)
        {
            var user = _mapper.Map<User>(model);
            var rowChanges = await _userRepo.AddAsync(user);

            return rowChanges != null ? (true, $"User: {model.FullName} was successfully created!") : (false, "Failed To create user!");
        }

        public async Task<(bool successful, string msg)> Update(UserVM model)
        {

            var user = await _userRepo.GetSingleByAsync(u => u.Id == model.UserId);
            if (user == null)
            {
                return (false, $"User with ID:{model.UserId} wasn't found");
            }

            var userupdate = _mapper.Map(model,user);
            var rowChanges = await _userRepo.UpdateAsync(userupdate);

            return rowChanges != null ?(true, $"User detail update was successful!") : (false, "Failed To save changes!");

        }


        public async Task<IEnumerable<DeleteUserVM>> GetUsers()
        {
            var users = await _userRepo.GetAllAsync();
            var userViewModels = users.Select(u => new DeleteUserVM
            {
                FullName = u.FullName,
                Email = u.Email,
                Id = u.Id,
                Password = u.Password,
            });
            return userViewModels;
        }

        public async Task<DeleteUserVM> GetUser(int userId)
        {
            var user = await _userRepo.GetSingleByAsync(u => u.Id == userId);
            var Auser = _mapper.Map<DeleteUserVM>(user);

            return Auser;
        }


        public async Task<IEnumerable<UserWithTaskVM>> GetUsersWithTasksAsync()
        {

            return (await _userRepo.GetAllAsync(include: u => u.Include(t => t.TodoList))).Select(u => new UserWithTaskVM
            {
                Id = u.Id,
                Fullname = u.FullName,
                Tasks = u.TodoList.Select(t => new TaskVM
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate.ToString("d"),
                    Priority = t.Priority.ToString(),
                    Status = t.IsDone ? "Done" : "Not Done"
                })
            });
            
        }


            public async Task<(bool successful, string msg)> DeleteAsync(int userId)
        {
            var user = await _userRepo.GetSingleByAsync(u => u.Id == userId);

            if (user == null)
            {
                return (false, $"User with user:{user.Id} wasn't found");
            }

            await _userRepo.DeleteAsync(user);
            return await _unitOfWork.SaveChangesAsync() >= 0 ? (true, $"{user.FullName} was deleted") : (false, $"Delete Failed");
        }


        public async Task<(bool successful, string msg)> AddOrUpdateAsync(DeleteUserVM model)
        {

            User user = await _userRepo.GetSingleByAsync(u => u.Id == model.Id);

            if (user== null || String.IsNullOrEmpty($"{user.Id}"))
            {
                var newuser = _mapper.Map<User>(model);
                var addCity = await _userRepo.AddAsync(newuser);

                return addCity != null ? (true, $"User: {model.FullName} was successfully created!") : (false, "Failed To create user!");
            }

            var userupdate = _mapper.Map(model, user);
            var rowChanges = await _userRepo.UpdateAsync(userupdate);

            return rowChanges != null ? (true, $"City Name update was successful!") : (false, "Failed To save changes!");

        }
    }
}
