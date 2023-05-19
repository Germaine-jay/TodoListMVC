using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.BLL.Interfaces;
using TodoList.BLL.Models;
using TodoList.DAL.Entities;

namespace TodoListMVC.Controllers
{
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


        public async Task<IActionResult> New()
        {
            /*var model = await _todoService.GetTask(userId, taskId);*/
            return View(new AddOrUpdateTaskVM());
        }


        public IActionResult DeleteAUser(int userId)
        {
            return View(new CreateUserVM { Id = userId });

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



        [HttpPost("{userid}/{taskId}")]
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



        [HttpGet("{userId}/{taskId}")]
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
