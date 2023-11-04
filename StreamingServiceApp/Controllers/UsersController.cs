using Microsoft.AspNetCore.Mvc;
using StreamingServiceApp.DbData;
using StreamingServiceApp.Models;

namespace StreamingServiceApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly DynamoDBUserRepository _userRepository;

        public UsersController(DynamoDBUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ViewResult Signin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(User userLogin)
        {
            var user = await _userRepository.GetUserByEmailAsync(userLogin.Email);
            TempData["UserId"] = user.UserId;
            TempData["UserEmail"] = user.Email;
            if (user == null)
            {
                TempData["LoginError"] = $"{userLogin.Email} does not exist";
                return View(userLogin);
            }
            return RedirectToAction("Index", "Movies");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.SaveUserAsync(user);
                return RedirectToAction("Signin");
            }
            return View(user);
        }
    }
}
