using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.BLL.Interfaces;
using TodoList.BLL.Models;
using TodoList.DAL.Entities;

namespace TodoListMVC.Controllers
{
    [Route("[controller]/[action]/{userid?}")]
    [AutoValidateAntiforgeryToken]
    public class TodoListController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITodoListService _todoService;
        private readonly IMapper _mapper;


        public TodoListController(IUserService userService, ITodoListService _todoService)
        {
            _userService = userService;
            this._todoService = _todoService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _userService.GetUsersWithTasksAsync();
            return View(model);
        }


        public async Task<IActionResult> New(int userId)
        {
            var model = await _userService.GetUser(userId);
            return View(new AddOrUpdateTaskVM { UserId = userId});
        }



        public async Task<IActionResult> UpdateStatus(int userId, int taskId)
        {
            var user = await _todoService.GetTask(userId, taskId);
            return View(user);

        }

        public IActionResult DeleteAUser(int userId)
        {
            return View(new DeleteUserVM { Id = userId });

        }



        [HttpPost]
        public async Task<IActionResult> Save(AddOrUpdateTaskVM model)
        {
            if (ModelState.IsValid)
            {

                var (successful, msg) = await _todoService.AddOrUpdateAsync(model);

                if (successful)
                {

                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Index");
                }

                //TempData["ErrMsg"] = msg; for both views and redirect to actions
                ViewBag.ErrMsg = msg;
                return View("New");

            }
            return View("New");
        }



        [HttpPost("{taskId}")]
        public async Task<IActionResult> DeleteTask(int userId, int taskId)
        {
            if (ModelState.IsValid)
            {
                var (success, msg) = await _todoService.DeleteTaskAsync(userId, taskId);

                if (success)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Index");
                }

                TempData["ErrMsg"] = msg;
                return RedirectToAction("Index");
            }
            return View("Index");
        }



        [HttpGet("{taskId}")]
        public async Task<IActionResult> SaveUpdateStatus(int userId, int taskId)
        {

            if (ModelState.IsValid)
            {
                var (successful, msg) = await _todoService.ToggleTaskStatus(userId, taskId);

                if (successful)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Index");
                }

                TempData["ErrMsg"] = msg;
                return View("Index");

            }
            return View("Index");
        }

    }

}
