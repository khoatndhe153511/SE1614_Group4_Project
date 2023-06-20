using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;
using SE1614_Group4_Project_API.Utils;
using SE1614_Group4_Project_API.Utils.Interfaces;
using System.Security.Claims;

namespace SE1614_Group4_Project_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ILogicHandler _logicHandler;

		public UserController(IUserRepository userRepository, IPostRepository postRepository, ILogicHandler logicHandler)
		{
			_userRepository = userRepository;
			_postRepository = postRepository;
			_logicHandler = logicHandler;
		}

        [HttpGet]
        public IActionResult GetAllUser()
        {
            try
            {
                return Ok(_userRepository.GetAll().Result);
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpGet("{uid}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                return Ok(_userRepository.Find(id));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpGet]
        public IActionResult UpdateUser(User user)
        {
            try
            {
                return Ok(_userRepository.Update(user));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            try
            {
                return Ok(_userRepository.Add(user));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpDelete("{uid}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                User? _ = _userRepository.Find(id);
                if (_ != null)
                {
                    _userRepository.Delete(_);
                }
                return Ok();
            }
            catch (DbUpdateException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword([FromBody] ChangePasswordModelDto model)
        {
            var currentUser = GetCurrentUser();
            if (currentUser is null)
            {
                return NotFound(new { errMess = Constants.ERR003 });
            }

            var user = _userRepository.findByName(currentUser.Name);
            if (user is null)
            {
                return NotFound(new { errMess = Constants.ERR002 });
            }

            if (user.Password != model.OldPassword)
            {
                return BadRequest(new { errMess = "Your old password is incorrect" });
            }
            else if (model.OldPassword == model.NewPassword)
            {
                return BadRequest(new { errMess = "You cannot set your new password same like old password!" });
            }
            else if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest(new
                {
                    errMess = "Confirm password failed!"
                });
            }

            try
            {
                user.Password = model.NewPassword;
                _userRepository.UpdatePassword(user.Email, model.NewPassword);
                Response.Cookies.Delete("ACCESS_TOKEN");
                return Ok(new
                {
                    successMess = "Password changed successfully!"
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var user = _userRepository.FindByEmail(model.Email);
            if (user is null)
            {
                return NotFound(Constants.ERR002);
            }
            string tempPass = _logicHandler.GeneratePassword(12);

            _logicHandler.SendEmailAsync(model.Email, "[Spriderum] Reset Password",
                $"Your new password is: {tempPass}.");

            _userRepository.UpdatePassword(model.Email, tempPass);
            return Ok("Password Reset Successfully!");
        }

        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity is not null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Role = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value)
                };
            }
            return null;
        }

        [HttpGet]
        [Authorize(Roles = "0, 1, 2, 3")]
        public IActionResult GetDetailProfile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity is not null)
            {
                var userClaims = identity.Claims;
                var name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value;

                var userProfile = _userRepository.findByName(name);

                var _totalPost = _postRepository.CountTotalPostByUserId(userProfile.Id);
                var _totalComment = _postRepository.CountTotalCommentByUserId(userProfile.Id);
                var _totalPoint = _postRepository.TotalPointByUserId(userProfile.Id);
                var _totalView = _postRepository.CountTotalViewByUserId(userProfile.Id);
                return Ok(new
                {
                    username = userProfile.Name,
                    avatar = userProfile.Avatar,
                    displayName = userProfile.DisplayName,
                    grAvatar = userProfile.Gravatar,
                    phoneNumber = userProfile.PhoneNumber,
                    email = userProfile.Email,
                    birth = userProfile.Birth,
                    gender = userProfile.Gender,
                    totalPost = _totalPost,
                    totalView = _totalView,
                    totalComment = _totalComment,
                    totalPoint = _totalPoint,
                });
            }
            return null;
        }

		[HttpGet]
		[Authorize(Roles = "0, 1, 2, 3")]
		public IActionResult GetListPostOfUser()
		{
			var identity = HttpContext.User.Identity as ClaimsIdentity;

			if (identity is not null)
			{
				var userClaims = identity.Claims;
				var name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value;

				var userProfile = _userRepository.findByName(name);

				var posts = _postRepository.GetAllPostsByUserId(userProfile.Id);

                return Ok(posts);
			}
			return null;
		}

		[HttpGet("{username}")]
		[Authorize(Roles = "0, 1, 2, 3")]
		public IActionResult CheckUsernameExist(string username)
		{
			var check = _userRepository.checkUsername(username);
			return Ok(check);
		}

		[HttpGet("{email}")]
		[Authorize(Roles = "0, 1, 2, 3")]
		public IActionResult CheckEmailExist(string email)
		{
			var check = _userRepository.checkEmail(email);
			return Ok(check);
		}
	}
}