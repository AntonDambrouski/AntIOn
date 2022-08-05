using IdentityServer.Models;
using IdentityServer.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(vm.Name, vm.Password, vm.RememberMe, false);
            if (result.Succeeded)
            {
                return Redirect(vm.ReturnUrl);
            }

            vm.Errors = new[]
            {
                new ViewError("Login error", "Login or password is incorrect!")
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new AppUser { UserName = vm.Name };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(vm.ReturnUrl);
            }

            vm.Errors = result.Errors.Select(e => new ViewError("Register error", e.Description));
            return View(vm);
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var action = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var props = _signInManager.ConfigureExternalAuthenticationProperties(provider, action);
            return Challenge(props, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                return RedirectToAction(nameof(Login), new { returnUrl });
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            var name = info.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var vm = new ExternalLoginModel
            {
                Name = name,
                ReturnUrl = returnUrl
            };

            return View("ExternalLoginConfirmation", vm);
        }

        public async Task<IActionResult> ExternalRegister(ExternalLoginModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("ExternalLoginConfirmation", vm);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                return RedirectToAction(nameof(Login), new { vm.ReturnUrl });
            }

            var user = await _userManager.FindByNameAsync(vm.Name);
            if (user is null)
            {
                user = new() { UserName = vm.Name };
                user.Email = info.Principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                user.PhoneNumber = info.Principal.FindFirst(ClaimTypes.MobilePhone)?.Value ?? string.Empty;
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    vm.Errors = createResult.Errors.Select(e => new ViewError("Register error", e.Description));
                    return View("ExternalLoginConfirmation", vm);
                }
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                vm.Errors = result.Errors.Select(e => new ViewError("Register error", e.Description));
                return View("ExternalLoginConfirmation", vm);
            }

            await _signInManager.SignInAsync(user, false);
            return Redirect(vm.ReturnUrl);
        }
    }
}
