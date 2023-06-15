using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SE1614_Group4_Project_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly spriderumContext _context;
        private readonly IConfiguration _config;

        public AuthController(spriderumContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public ActionResult Login([FromForm] UserLoginDTO loginModel)
        {
            var user = _context.Users.Where(x => x.Name.Equals(loginModel.Name) && x.Password.Equals(loginModel.Password))
                .FirstOrDefault();
            if (user != null)
            {
                var tokenStr = GenerateJSONWebToken(user);
                Response.Cookies.Append("ACCESS_TOKEN", tokenStr);
                return Ok(new { token = tokenStr });
            }
            else
            {
                return NotFound("User does not exist!");
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("ACCESS_TOKEN");
            return Ok();
        }

        [HttpPost]
        public ActionResult<User> SignUp([FromForm] UserRegisterDto newUser)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Avatar = null,
                DisplayName = newUser.DisplayName,
                Gravatar = null,
                Name = newUser.Name,
                Role = (int)Constants.Role.User,
                Password = newUser.Password
            };

            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userInfo.Role.ToString())
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