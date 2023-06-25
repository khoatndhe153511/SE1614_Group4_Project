using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Linq;

namespace SE1614_Group4_Project_API.Controllers.Auth
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly spriderumContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(spriderumContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult Login([FromBody] UserLoginDto loginModel)
        {
            try
            {
                var user = _context.Users
                    .FirstOrDefault(x => (x.Email.Equals(loginModel.Email) || x.Name.Equals(loginModel.Email)) 
                                         && x.Password.Equals(loginModel.Password));
                if (user == null) return NotFound("Email or password is wrong!");
                var tokenStr = GenerateJsonWebToken(user);
                Response.Cookies.Append("ACCESS_TOKEN", tokenStr);
                return Ok(new { token = tokenStr });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("ACCESS_TOKEN");
                return Ok("Logout Successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<User> SignUp([FromBody] UserRegisterDto newUser)
        {
            var existedUser = _context.Users
                .Where(x => x.Email.Equals(newUser.Email) || x.Name.Equals(newUser.UserName));

            if (existedUser != null) return BadRequest("Username or email already taken");
            if (newUser.Password != newUser.ConfirmPassword)
            {
                return BadRequest("Confirm Password Failed!");
            }

            try
            {
                var newMem = _mapper.Map<User>(newUser);
                _context.Users.Add(newMem);
                _context.SaveChanges();
                return Ok(newMem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJsonWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, userInfo.Name),
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(ClaimTypes.Role, userInfo.Role.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }

		[HttpGet]
        [Route("/api/google-login")]
		public IActionResult GoogleLogin()
		{
			var properties = new AuthenticationProperties
			{
				RedirectUri = Url.Action(nameof(GoogleCallback))
			};

			return Challenge(properties, GoogleDefaults.AuthenticationScheme);
		}

		[HttpGet]
        [Route("/api/google-callback")]
		public async Task<IActionResult> GoogleCallback()
		{
			var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

			// Handle the authentication result, retrieve user information, and generate a JWT token

			// Example: If authentication is successful, generate a JWT token and return it to the client
			if (authenticateResult.Succeeded)
			{
				// Retrieve user information from authenticateResult and generate a JWT token
				// Return the JWT token to the client
				// Example:
				var token = GenerateJwtToken(authenticateResult.Principal.Identity.Name);
				return Ok(new { Token = token });
			}
			else
			{
				// Authentication failed
				return BadRequest("Authentication failed");
			}
		}

		// Helper method to generate a JWT token
		private string GenerateJwtToken(string username)
		{
			// Generate a JWT token using a library like System.IdentityModel.Tokens.Jwt
			// Example:
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // Replace with your own secret key

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.Name, username),
			};

			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Issuer"],
				claims,
				expires: DateTime.UtcNow.AddMinutes(120),
				signingCredentials: credentials);

			var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
			return encodeToken.ToString();
		}
	}

}