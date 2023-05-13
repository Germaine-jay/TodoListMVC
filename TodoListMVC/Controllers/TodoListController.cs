using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.BLL.Interfaces;
using TodoList.BLL.Models;
using TodoList.DAL.Entities;

namespace TodoListMVC.Controllers
{

    // [Route("[controller]/[action]/{id?}")]
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


        public IActionResult New()
        {
            return View(new AddOrUpdateTaskVM());
        }



        public async Task<IActionResult> UpdateStatus(int userId, int taskId)
        {
            //var user = await _userService.GetUser(userId);
            return View(new AddOrUpdateTaskVM { UserId = userId, TaskId = taskId });

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

        [HttpPut]
        public async Task<IActionResult> SaveUpdate (UserVM model)
        {           

            if (ModelState.IsValid)
            {   
                var (successful, msg) = await _userService.Update(model);
                
                if (successful)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("AllUsers");
                }

                TempData["ErrMsg"] = msg;
                return View("AllUsers");

            }
            return View("AllUsers");
        }



        /*[HttpGet("{userid}/{taskId}")]
        public async Task<IActionResult> Delete(int userId, int taskId)
        {
            var (success, msg) = await _todoService.DeleteAsync(userId, taskId);

            if (success)
            {
                TempData["SuccessMsg"] = msg;
                return RedirectToAction("Index");
            }

            TempData["ErrMsg"] = msg;
            return RedirectToAction("Index");
        }*/



        [HttpPut("{userId}/{taskId}")]
        public async Task<IActionResult> SaveUpdateStatus(int userId, int taskId)
        {

            if (ModelState.IsValid)
            {
                var (successful, msg) = await _todoService.ToggleTaskStatus(userId, taskId);

                if (successful)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Index", new { userId });
                }

                TempData["ErrMsg"] = msg;
                return View("Index");

            }
            return View("Index");
        }

    }

}
