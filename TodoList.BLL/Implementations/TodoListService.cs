using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoList.BLL.Interfaces;
using TodoList.BLL.Models;
using TodoList.DAL.Entities;
using TodoList.DAL.Repository;

namespace TodoList.BLL.Implementations
{
    public class TodoListService : ITodoListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Todo> _taskRepo;


        public TodoListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _taskRepo = _unitOfWork.GetRepository<Todo>();
            _userRepo = _unitOfWork.GetRepository<User>();
        }

        public async Task<(bool successful, string msg)> AddOrUpdateAsync(AddOrUpdateTaskVM model)
        {

            User user = await _userRepo.GetSingleByAsync(u => u.Id == model.UserId, include: u => u.Include(x => x.TodoList), tracking: true);

            if (user == null)
            {
                return (false, $"User with id:{model.UserId} wasn't found");
            }

            Todo task = user.TodoList.SingleOrDefault(t => t.Id == model.TaskId);
            if (task != null)
            {
                _mapper.Map(model, task);

                await _unitOfWork.SaveChangesAsync();
                return (true, "Update Successful!");
            }

            var newTask = _mapper.Map<Todo>(model);
            user.TodoList.Add(newTask);

            var rowChanges = await _unitOfWork.SaveChangesAsync();

            return rowChanges > 0 ? (true, $"Task: {model.Title} was successfully created!") : (false, "Failed To save changes!");
        }


        public async Task<(bool successful, string msg)> DeleteTaskAsync(int userId, int taskId)
        {
            User user = await _userRepo.GetSingleByAsync(u => u.Id == userId, include: u => u.Include(x => x.TodoList), tracking: true);
            if (user == null)
            {
                return (false, "User with ID{user.Id} not found");
            }

            var task = user?.TodoList?.SingleOrDefault(u => u.Id == taskId);

            if (task != null)
            {
                await _taskRepo.DeleteAsync(task);
                return (true, $"task with taskId{taskId} Deleted");
            }
            return (false, $"Task with id:{taskId} was not found");
        }


        public async Task<TaskVM> GetTask(int userId, int taskId)
        {

            var user = await _userRepo.GetSingleByAsync(u => u.Id == userId, include: u => u.Include(x => x.TodoList), tracking: true);
            if (string.IsNullOrEmpty(user.ToString()))
            {
                var task = user?.TodoList?.SingleOrDefault(u => u.Id == taskId);
                if (string.IsNullOrEmpty(task.ToString()))
                {              
                    var userTask = _mapper.Map<TaskVM>(task);
                    return userTask;
                }
            }
                return null;
        }

        public async Task<(bool successful, string msg)> ToggleTaskStatus(int userId, int taskId)
        {
            User user = await _userRepo.GetSingleByAsync(u => u.Id == userId, include: u => u.Include(x => x.TodoList), tracking: true);

            if (user != null)
            {
                Todo task = user.TodoList.SingleOrDefault(t => t.Id == taskId);
                if (task != null)
                {
                    task.IsDone = !task.IsDone;
                    var row = _mapper.Map<Todo>(task);

                    var rowChanges = await _taskRepo.UpdateAsync(row);
                    return rowChanges != null ? (true, "Task updated") : (false, "Task not updated");
                }
                return (false, "Task not found or does not belong to user");
            }
            return (false, $"User with the ID {userId} not found");
        }

    }
}
