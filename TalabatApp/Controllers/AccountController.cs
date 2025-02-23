using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TalabatApp.Core.Entities.Identity;
using TalabatApp.Core.Services.Contract;
using TalabatApp.Dtos;
using TalabatApp.Errors;
using TalabatApp.Extensions;

namespace TalabatApp.Controllers
{
  
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
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
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });

        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto Form)
        {

            if (CheckEmailExist(Form.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email is already Exist!" } });




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
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });

        }


        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("address")]
        public async Task<ActionResult<Address>> GetUserAddress()
        { 
            var user = await _userManager.FindUserWithAddressByEmailAsync(User);

            return Ok(user.Address);
        }


        [HttpGet("emailExist")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }




    }
}
