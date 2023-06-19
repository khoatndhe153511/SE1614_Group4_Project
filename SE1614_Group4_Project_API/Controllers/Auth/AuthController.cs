using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SE1614_Group4_Project_API.Controllers.Auth
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly spriderumContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(spriderumContext context, IConfiguration config, IMapper mapper,
            IUserRepository userRepository)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult Login([FromBody] UserLoginDTO loginModel)
        {
            try
            {
                var user = _context.Users
                            .Where(x => x.Email.Equals(loginModel.Email) && x.Password.Equals(loginModel.Password))
                            .FirstOrDefault();
                if (user != null)
                {
                    var tokenStr = GenerateJSONWebToken(user);
                    Response.Cookies.Append("ACCESS_TOKEN", tokenStr);
                    return Ok(new { token = tokenStr });
                }
                else
                {
                    return NotFound("Email or password is wrong!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
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
                .Where(x => x.Email.Equals(newUser.Email) && x.Name.Equals(newUser.UserName))
                .FirstOrDefault();

            if (existedUser == null)
            {
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
            return BadRequest("Username or email already taken");
        }

        private string GenerateJSONWebToken(User userInfo)
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
            return encodeToken.ToString();
        }
    }
}