using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using TodoList.BLL.Interfaces;
using TodoList.BLL.Models;
using TodoList.DAL.Database;
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

            // _taskRepo.GetSingleByAsync(t => t.UserId == model.UserId && t.Id == model.TaskId);

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

            // var newTask = _mapper.Map<AddOrUpdateTaskVM,Todo>(model);
            var newTask = _mapper.Map<Todo>(model);

            user.TodoList.Add(newTask);

            var rowChanges = await _unitOfWork.SaveChangesAsync();

            return rowChanges > 0 ? (true, $"Task: {model.Title} was successfully created!") : (false, "Failed To save changes!");

        }


        public Task<(bool successful, string msg)> DeleteAsync(int userId, int taskId)
        {
            /*var user = ToDoListDbContext.GetUsersWithToDos().SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return (false, "User with ID{user.Id} not found");
            }

            var task = user.TodoList.SingleOrDefault(u => u.Id == taskId);

            if (task == null)
            {
                user.TodoList.Remove(task);
                return (true, $"task with taskId{taskId}not found");
            }

            return (false, $"Task with id:{taskId} was not found");*/
            throw new NotImplementedException();

        }

        public (Todo to, string msg)  GetTask(int userId, int taskId)
        {
            //var user = ToDoListDbContext.GetUsersWithToDos().SingleOrDefault(u => u.Id == userId);

            /*var user = await _userRepo.GetSingleByAsync(u => u.Id == userId, include: u => u.Include(x => x.TodoList), tracking: true);
            if (user == null)
            {
                return (null, "User not found");
            }
            var task = user.TodoList.SingleOrDefault(u => u.Id == taskId);

            if (task == null)
            {
                return (null, "task not found");
            }

            return (task, "");*/
            throw new NotImplementedException();


        }

        public async Task<IEnumerable<TaskVM>> GetTodoList(int userId)
        {
            /*var todos = ToDoListDbContext.GetUsersWithToDos().SelectMany(u=> u.TodoList).ToList();
            return todos;*/

         
            throw new NotImplementedException();

        }

        /*public async Task<(bool Done, string msg)> ToggleTaskStatus(int userId, int taskId)
        {
            var user = await _userRepo.GetSingleByAsync(u => u.Id == userId, include: u => u.Include(x => x.TodoList), tracking: true);

            if (user != null)
            {
                Todo task = user.TodoList.SingleOrDefault(t => t.Id == taskId);
                if (task != null)
                {
                    task.IsDone = !task.IsDone;

                    await _userRepo.UpdateAsync(user);

                    var rowChanges = await _unitOfWork.SaveChangesAsync();
                    return rowChanges > 0 ? (true, "Task updated") : (false, "Task not updated");
                }
                return (false, $"Task with the id {taskId} not found");
            }
            return (false, $"User with the id {userId} not found");
        }*/

        public async Task<(bool Done, string msg)> ToggleTaskStatus(int userId, int taskId)
        {
            var user = await _userRepo.GetSingleByAsync(u => u.Id == userId, tracking: true);

            if (user != null)
            {
                
                Todo task = user.TodoList.SingleOrDefault(t => t.Id == taskId);
                if (task != null && task.UserId == userId)
                {
                    _mapper.Map<TaskVM>(task);
                    task.IsDone = !task.IsDone;

                    var rowChanges = await _unitOfWork.SaveChangesAsync();
                    return rowChanges > 0 ? (true, "Task updated") : (false, "Task not updated");
                }
                return (false, "Task not found or does not belong to user");
            }
            return (false, $"User with the ID {userId} not found");
        }



    }
}
