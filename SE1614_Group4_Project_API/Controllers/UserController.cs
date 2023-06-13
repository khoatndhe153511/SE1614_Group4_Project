﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository;
using SE1614_Group4_Project_API.Repository.Interfaces;

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
    }
}