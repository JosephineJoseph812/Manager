using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Entities;

namespace taskk.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new IdentityUserLogin());
        }

        [HttpPost]
        public async Task<IActionResult> Login(IdentityUserLogin loginDto)
        {
            if (!ModelState.IsValid) return View(loginDto);
            var user = await _userManager.Users.SingleOrDefaultAsync(X => X.Email == loginDto.UserId);
            if (user == null) return RedirectToAction(nameof(Login));
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.UserId, true, false);
            if (!result.Succeeded) return View(loginDto);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterDto());
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return View(registerDto);

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("Email", "The Email is Taken");
                return View(registerDto);
            }

            var user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                CreatedAt = DateTime.UtcNow,
                //RegisteredDate = DateTime.UtcNow,
                EmailConfirmed = false,

            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return View(registerDto);

            await _userManager.AddToRoleAsync(user, "Customer");

            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}

