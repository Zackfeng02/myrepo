using Microsoft.AspNetCore.Mvc;
using StreamingServiceApp.Models;
using StreamingServiceApp.DbData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using StreamingServiceApp.ViewModel;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace StreamingServiceApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(User userLogin)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLogin.Email);
            if (user == null)
            {
                TempData["LoginError"] = $"{userLogin.Email} does not exist";
                return View(userLogin);
            }
            if (!VerifyPassword(user.Password, userLogin.Password))
            {
                TempData["LoginError"] = "Incorrect password";
                return View(userLogin);
            }
            // Create the security claims with user information
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            // Create the claims identity
            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuthenticationDefaults");

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


            return RedirectToAction("Index", "Movies");
        }


        [HttpGet]
        public IActionResult Signup()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserId = model.UserId,
                    Email = model.Email,
                    Password = HashPassword(model.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Signin");
            }
            return View(model);
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        private bool VerifyPassword(string hashedPasswordWithSalt, string passwordToVerify)
        {
            var parts = hashedPasswordWithSalt.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var passwordHash = parts[1];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordToVerify,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return passwordHash == hashed;
        }

    }
}
