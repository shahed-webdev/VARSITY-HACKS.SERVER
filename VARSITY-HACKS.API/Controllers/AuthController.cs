using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VARSITY_HACKS.BusinessLogic.Registration;
using VARSITY_HACKS.DATA;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IRegistrationCore _registration;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration config,SignInManager<IdentityUser> signInManager, IRegistrationCore registration)
        {
            _userManager = userManager;
            _config = config;
            _signInManager = signInManager;
            _registration = registration;
        }

        // POST api/Auth/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            var user = new IdentityUser() { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ResponseModel(false, result.Errors.First().Description));
            }

            //var createModel = new RegistrationCreateModel
            //{
            //    UserName = model.Email,
            //    Name = model.Name
            //};

            await _registration.CreateAsync(model.Name, model.Email);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.Email),
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Email, model.Email),
            };

            var token = new JwtSecurityToken
            (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new ResponseModel<string>(true, "Token", tokenString));
        }


        // POST api/Auth/Login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var result =
                await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                return BadRequest(new ResponseModel(false, "Incorrect username or password"));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.UserName),
                new Claim(ClaimTypes.Email, model.UserName),
            };

            var token = new JwtSecurityToken
            (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new ResponseModel<string>(true, "Token", tokenString));
        }

        //jwt token based external login
        [AllowAnonymous]
        [HttpPost("external-login")]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            properties.AllowRefresh = true;

            return Challenge(properties, provider);
        }


        //ExternalLoginCallback jwt
        [AllowAnonymous]
        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl, string? remoteError = null)
        {
            if (remoteError != null)
            {
                return BadRequest(new ResponseModel(false, "Error from external provider: " + remoteError));
            }
            
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest(new ResponseModel(false, "Error loading external login information."));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, email),
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email),
            };
            var token = new JwtSecurityToken
            (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])), SecurityAlgorithms.HmacSha256)
            );

            if (result.Succeeded)
            {
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new ResponseModel<string>(true, "Token", tokenString));
            }
            else
            {
               
                var user = new IdentityUser() { UserName = email, Email = email };
                var createResult = await _userManager.CreateAsync(user);
               
                if (!createResult.Succeeded)
                {
                    return BadRequest(new ResponseModel(false, "Error creating user"));
                }
                
                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                
                if (!addLoginResult.Succeeded)
                {
                    return BadRequest(new ResponseModel(false, "Error adding external login"));
                }
                
                await _signInManager.SignInAsync(user, false);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new ResponseModel<string>(true, "Token", tokenString));
               
            }
        }



        // POST api/Auth/logout
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new ResponseModel(true, "Sign Out Successfully"));
        }

        // GET api/Auth/getUsers
        [HttpGet("getUser")]
        public async Task<IActionResult> GetUser()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _registration.GetUserAsync(userName);
            
            if (!response.IsSuccess) return BadRequest(response.Message);
            return Ok(response);
        }

        // PUT api/Auth/updateUser
        [HttpPut("updateUser")]
        public async Task<IActionResult> PutUser([FromForm] RegistrationEditModelWithFormFile model)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");
            
            if (model.FormFile != null && model.FormFile.Length > 0)
            {
                model.Image = await model.FormFile.GetBytesAsync();
            }

            var response = await _registration.EditAsync(userName, model);
            
            if (!response.IsSuccess) return BadRequest(response.Message);
           
            return Ok(response);
        }


        // GET api/Auth/get-mode
        [HttpGet("get-mode")]
        public async Task<IActionResult> GetMode()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _registration.GetModeAsync(userName);

            if (!response.IsSuccess) return BadRequest(response.Message);
            return Ok(response);
        }

        // GET api/Auth/get-mode
        [HttpPut("set-mode")]
        public async Task<IActionResult> SetMode([FromQuery]UserMode mode)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _registration.SetModeAsync(userName, mode);

            if (!response.IsSuccess) return BadRequest(response.Message);
            return Ok(response);
        }

    }
}