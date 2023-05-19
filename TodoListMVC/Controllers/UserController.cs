using Microsoft.AspNetCore.Mvc;
using TodoList.BLL.Interfaces;
using TodoList.BLL.Models;

namespace TodoListMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> NewUser(int userId)
        {
            var user = await _userService.GetUser(userId);
            return View(user);
        }


        public async Task<IActionResult> UpdateUser(int userId)
        {
            var user = await _userService.GetUser(userId);
            return View(user);
        }

        public async Task<IActionResult> AllUsers()
        {
            var model = await _userService.GetUsers();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SaveUser(CreateUserVM model)
        {

            if (ModelState.IsValid)
            {
                var (successful, msg) = await _userService.AddOrUpdateAsync(model);

                if (successful)
                {

                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("AllUsers");
                }

                TempData["ErrMsg"] = msg;
                return View("NewUser");
            }

            return View("NewUser");
        }


        [HttpPost("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (ModelState.IsValid)
            {

                var (success, msg) = await _userService.DeleteAsync(userId);
                if (success)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("AllUsers");
                }

                TempData["ErrMsg"] = msg;
                return RedirectToAction("AllUsers");

            }
            return View("AllUsers");

        }


        [HttpPut]
        public async Task<IActionResult> SaveUpdate(UserVM model)
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

    }
}
