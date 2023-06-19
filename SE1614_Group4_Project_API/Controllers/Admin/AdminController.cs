using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE1614_Group4_Project_API.Models;

namespace SE1614_Group4_Project_API.Controllers.Admin
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private readonly spriderumContext _context;

		public AdminController(spriderumContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Authorize(Roles = "0")]
		public IActionResult GetAllUser()
		{
			return Ok();
		}
	}
}
