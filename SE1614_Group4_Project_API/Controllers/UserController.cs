﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;
using SE1614_Group4_Project_API.Utils;
using System.Security.Claims;

namespace SE1614_Group4_Project_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
        public ActionResult ChangePassword([FromBody] ChangePasswordModelDto model)
        {
            var currentUser = GetCurrentUser();
            if (currentUser is null)
            {
                return BadRequest(Constants.ERR003);
            }
            else
            {
                var user = _userRepository.Find(currentUser.Email);
                if (user is null)
                {
                    return NotFound(Constants.ERR002);
                }
                else
                {
                    if (user.Password != model.OldPassword)
                    {
                        return BadRequest("Your old password is incorrect");
                    }
                    else if (model.OldPassword == model.NewPassword)
                    {
                        return BadRequest("You cannot set your new password same like old password!");
                    }
                    else if (model.NewPassword != model.ConfirmPassword)
                    {
                        return BadRequest("Confirm password fail!");
                    }
                    else
                    {
                        try
                        {
                            user.Password = model.NewPassword;
                            _userRepository.Update(user);
                        }
                        catch (Exception e)
                        {
                            return BadRequest(e.Message);
                        }
                    }
                }
            }
            return Ok("Password changed successfully!");
        }

        [HttpPost]
        public ActionResult ForgotPassword([FromBody] string email)
        {
            var user = _userRepository.Find(email);
            if (user is null)
            {
                return NotFound(Constants.ERR002);
            }
            else
            {
                // send mail, change password
            }
            return Ok();
        }

        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity is not null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Role = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value)
                };
            }
            return null;
        }
    }
}