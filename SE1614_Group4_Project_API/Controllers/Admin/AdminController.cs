using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository;
using SE1614_Group4_Project_API.Repository.Interfaces;
using SE1614_Group4_Project_API.Utils;
using System.Security.Claims;

namespace SE1614_Group4_Project_API.Controllers.Admin
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AdminController : Controller

	{
		private readonly spriderumContext _spriderumContext;

		private readonly IUserRepository _userRepository;

		public AdminController(spriderumContext spriderumContext, IUserRepository userRepository)
		{
			_spriderumContext = spriderumContext;
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

		[HttpGet]
		[Authorize(Roles = "0")]
		public async Task<IActionResult> GetListUser(int page, int pageSize)
		{
				var query = _spriderumContext.Users
					.OrderByDescending(x => x.DisplayName)
					.Select(x => new User
					{
						Id = x.Id,
						Avatar = x.Avatar,
						Name= x.Name,
						DisplayName = x.DisplayName,
						Role= x.Role,
						PhoneNumber = x.PhoneNumber,
						Email = x.Email,
						Birth = x.Birth,
						Gender = x.Gender,
					});

				var totalCount = await query.CountAsync();

				var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
				var currentPage = Math.Min(page, totalPages);

				var users = await query.Skip((currentPage - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();

				var result = new PageResult<User>
				{
					TotalPage = totalPages,
					TotalCount = totalCount,
					Page = currentPage,
					PageSize = pageSize,
					Results = users.Select(x => new User
					{
						Id = x.Id,
						Avatar = x.Avatar,
						Name = x.Name,
						DisplayName = x.DisplayName,
						Role = x.Role,
						PhoneNumber = x.PhoneNumber,
						Email = x.Email,
						Birth = x.Birth,
						Gender = x.Gender,
					})
				};

				return Ok(result);
			

	
		}
	}
}
