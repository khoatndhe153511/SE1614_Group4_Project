using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Controllers.Admin
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AdminController : Controller

	{

		private readonly IUserRepository _userRepository;

		public AdminController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		//[HttpGet]
		//[Authorize(Roles = "0")]
		//public IActionResult GetAllUser()
		//{
		//	var users = _context.Users.ToList();

		//	return Ok(users);
		//}


		[HttpPost]
		[Authorize(Roles ="0")]
		public IActionResult updateRole([FromBody] ChangeRoleDTO changeRoleDTO)
		{
			var user = _userRepository.updateRole(changeRoleDTO.username, changeRoleDTO.role);
			return Ok(user);
		}
	}
}
