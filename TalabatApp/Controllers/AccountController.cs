using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TalabatApp.Core.Entities.Identity;
using TalabatApp.Dtos;
using TalabatApp.Errors;

namespace TalabatApp.Controllers
{
  
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]  // POST : /api/Account/login

        public async Task<ActionResult<UserDto>> Login(LoginDto Form)
        {
            var user = await _userManager.FindByEmailAsync(Form.Email);
            if (user is null) return Unauthorized(new ApiErrorResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, Form.Password, false);
            if (result.Succeeded is false) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "this Will be Token"
            });

        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto Form)
        {
            // Create User
            var user = new AppUser()
            {
                DisplayName = Form.DisplayName,
                Email = Form.Email,
                UserName = Form.Email.Split("@")[0],
                PhoneNumber = Form.PhoneNumber
            };

            // Add User To Database

            var Result = await _userManager.CreateAsync(user, Form.Password);

            // return Data 

            if (Result.Succeeded is false) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "this will be token"
            });

        }

 
    }
}
