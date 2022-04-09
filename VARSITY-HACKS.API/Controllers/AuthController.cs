using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VARSITY_HACKS.BusinessLogic;
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
        private readonly IExternalAuthService _externalAuthService;
        private readonly IEmailSender _emailSender;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration config, SignInManager<IdentityUser> signInManager, IRegistrationCore registration, IExternalAuthService externalAuthService, IEmailSender emailSender)
        {
            _userManager = userManager;
            _config = config;
            _signInManager = signInManager;
            _registration = registration;
            _externalAuthService = externalAuthService;
            _emailSender = emailSender;
        }

        // POST api/Auth/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel(false, ModelState.Values.FirstOrDefault()!.Errors.FirstOrDefault()!.ErrorMessage));

            var user = new IdentityUser() { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ResponseModel(false, result.Errors.First().Description));
            }

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
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])), SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new ResponseModel<string>(true, "Token", tokenString));
        }


        // POST api/Auth/Login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
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
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])), SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new ResponseModel<string>(true, "Token", tokenString));
        }

        //External Facebook Login jwt
        [AllowAnonymous]
        [HttpPost("FacebookLogin")]
        public async Task<IActionResult> FacebookLogin([FromBody] string accessToken)
        {
            try
            {
                var validateTokenResult = await _externalAuthService.ValidateFacebookAccessTokenAsync(accessToken);

                if (!validateTokenResult.Data.IsValid) return BadRequest(new ResponseModel(false, "Error from facebook provider"));

                var userInfo = await _externalAuthService.GetFacebookUserInfoAsync(accessToken);



                var user = await _userManager.FindByEmailAsync(userInfo.Email);

                var email = userInfo.Email;

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, email),
                new Claim(ClaimTypes.Name, userInfo.Name),
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

                if (user != null)
                {
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new ResponseModel<string>(true, "Token", tokenString));
                }
                else
                {

                    var newUser = new IdentityUser() { UserName = email, Email = email };
                    var createResult = await _userManager.CreateAsync(newUser);
                    await _registration.CreateAsync(userInfo.Name, email);
                    if (!createResult.Succeeded)
                    {
                        return BadRequest(new ResponseModel(false, "Error creating user"));
                    }

                    //var addLoginResult = await _userManager.AddLoginAsync(newUser, info);

                    //if (!addLoginResult.Succeeded)
                    //{
                    //    return BadRequest(new ResponseModel(false, "Error adding external login"));
                    //}

                    await _signInManager.SignInAsync(newUser, false);

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new ResponseModel<string>(true, "Token", tokenString));

                }
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel(false, $"{e.Message}. {e.InnerException?.Message ?? ""}"));
            }
            
        }

        //External Google Login jwt
        [AllowAnonymous]
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] string accessToken)
        {
            var validateTokenResult = await _externalAuthService.ValidateGoogleUserInfoAsync(accessToken);

            if (validateTokenResult.email_verified != "true") return BadRequest(new ResponseModel(false, "Error from google provider"));

            var email = validateTokenResult.email;
            var name = validateTokenResult.name;
            var user = await _userManager.FindByEmailAsync(email);



            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, email),
                new Claim(ClaimTypes.Name, name),
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

            if (user != null)
            {
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new ResponseModel<string>(true, "Token", tokenString));
            }
            else
            {

                var newUser = new IdentityUser() { UserName = email, Email = email };
                var createResult = await _userManager.CreateAsync(newUser);
                await _registration.CreateAsync(name, email);
                if (!createResult.Succeeded)
                {
                    return BadRequest(new ResponseModel(false, "Error creating user"));
                }

                await _signInManager.SignInAsync(newUser, false);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new ResponseModel<string>(true, "Token", tokenString));

            }
        }



        // POST api/Auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new ResponseModel(true, "Sign Out Successfully"));
        }

        // POST api/Auth/ForgotPassword
        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel(false, ModelState.Values.FirstOrDefault()!.Errors.FirstOrDefault()!.ErrorMessage));

                var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
                if (user == null)
                    return BadRequest(new ResponseModel(false, $"{forgotPasswordModel.Email} not valid email"));

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var tokenEncoded = Uri.EscapeDataString(token);
                var sb = new StringBuilder();

                sb.Append("Hi,<br/> Click on below given link to Reset Your Password<br/>");
                sb.Append($"<a href='{forgotPasswordModel.ResetPasswordUrl}?token={tokenEncoded}&email={user.Email}'>Click here to change your password</a><br/>");
                sb.Append("<b>Thanks</b>,<br> Varsity Hacks <br/>");

                var message = new Message(new string[] { user.Email }, "Reset password token", sb.ToString());

                await _emailSender.SendEmailAsync(message);

                return Ok(new ResponseModel(true, "Reset password token sent to your email"));
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel(false, $"{e.Message}. {e.InnerException?.Message ?? ""}"));
            }
           
        }

        // POST api/Auth/ResetPassword
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel(false, ModelState.Values.FirstOrDefault()!.Errors.FirstOrDefault()!.ErrorMessage));

            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);

            if (user == null)
                return BadRequest(new ResponseModel(false, $"{resetPasswordModel.Email} not valid email"));

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);

            if (!resetPassResult.Succeeded)
                return BadRequest(new ResponseModel(false, resetPassResult.Errors.FirstOrDefault()!.Description));


            return Ok(new ResponseModel(true, "Password reset successfully"));
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
        public async Task<IActionResult> SetMode([FromQuery] UserMode mode)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _registration.SetModeAsync(userName, mode);

            if (!response.IsSuccess) return BadRequest(response.Message);
            return Ok(response);
        }

    }
}