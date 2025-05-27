using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using technicalTes_Nawatech.Models;
using technicalTes_Nawatech.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace technicalTes_Nawatech.Controllers
{
    public class AccountController : Controller
    {
		private readonly SignInManager<Users> signInManager;
		private readonly UserManager<Users> userManager;

		public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
		}
        public IActionResult Login()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

				if(result.Succeeded)
				{
					return RedirectToAction("Index", "Product");
				}
				else
				{
					ModelState.AddModelError("", "Email or password is incorrect.");
					return View(model);
				}
			}
			return View(model);
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid) 
		{
				Users users = new Users
				{
					FullName = model.Name,
					Email = model.Email,
					UserName = model.Email,
				};

				var result = await userManager.CreateAsync(users, model.Password);

				if (result.Succeeded)
				{
					return RedirectToAction("Login", "Account");
				} else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}

					return View(model);
				}
			}
            return View(model);
        }

		public IActionResult VerifyEmail()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
		{
			if(ModelState.IsValid)
			{
				var user = await userManager.FindByNameAsync(model.Email);

				if (user == null)
				{
					ModelState.AddModelError("", "Something is wrong!");
					return View(model);
				}
				else
				{
					return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
				}
			}
			return View(model);
		}
		public IActionResult ChangePassword(string username)
		{
			if (string.IsNullOrEmpty(username))
			{
				return RedirectToAction("VerifyEmail", "Account");
			}
			return View(new ChangePasswordViewModel { Email = username });
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByNameAsync(model.Email);
				if (user != null)
				{
					var result = await userManager.RemovePasswordAsync(user);
					if (result.Succeeded)
					{
						result = await userManager.AddPasswordAsync(user, model.NewPassword);
						return RedirectToAction("Login", "Account");
					}
					else
					{
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
						return View(model);
					}
				}
				else
				{
					ModelState.AddModelError("", "Email not found!");
					return View(model);
				}
			}
			else
			{
				ModelState.AddModelError("", "Something went wrong. try again");
				return View(model);
			}
		}

		public async Task<IActionResult> Logout ()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home" );
		}
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new EditProfileViewModel
            {
                FullName = user.FullName,
                ExistingProfilePicture = user.ProfilPicture
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                user.FullName = model.FullName;

                if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile_pics");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfilePicture.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePicture.CopyToAsync(fileStream);
                    }

                    user.ProfilPicture = uniqueFileName;
                }

                await userManager.UpdateAsync(user);
                return RedirectToAction("Index", "Product");
            }

            model.ExistingProfilePicture = user.ProfilPicture;
            return View(model);
        }

    }
}
