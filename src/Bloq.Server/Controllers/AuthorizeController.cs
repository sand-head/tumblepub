using Bloq.Database.Models;
using Bloq.Shared.Models;
using Bloq.Shared.Models.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Bloq.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly UserManager<BloqUser> _userManager;
        private readonly SignInManager<BloqUser> _signInManager;

        public AuthorizeController(UserManager<BloqUser> userManager, SignInManager<BloqUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult CurrentUser()
        {
            return Ok(new UserModel(
                Username: User.Identity.Name,
                IsAuthenticated: User.Identity.IsAuthenticated,
                Claims: User.Claims.ToDictionary(c => c.Type, c => c.Value)));
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = new BloqUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return Ok(new RegisterResponse(false, result.Errors.Select(e => e.Description)));

            return await Login(new LoginModel(model.Email, model.Password, false));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return BadRequest("The requested user does not exist.");
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return BadRequest("The given password was incorrect.");

            await _signInManager.SignInAsync(user, model.RememberMe);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
