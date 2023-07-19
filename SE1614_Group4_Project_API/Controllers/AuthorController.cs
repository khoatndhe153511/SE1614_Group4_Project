using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Controllers
{
	[ApiController]
	[Route("api/[Controller]/[action]")]
	public class AuthorController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly spriderumContext _spriderumContext;

		public AuthorController(IUserRepository userRepository, spriderumContext spriderumContext)
		{
			_userRepository = userRepository;
			_spriderumContext = spriderumContext;
		}

		[HttpGet]
		public IActionResult getAuthorById(string id) 
		{
			var author = _userRepository.findById(id);
			return Ok(new
			{
				AuthorName = author.Name,
				AuthorEmail = author.Email,
				AuthorId = author.Id,
				AuthorDisplayName = author.DisplayName,
				AuthorAvatar = author.Avatar,
			});
		}

		[HttpGet]
		public IActionResult getAllPostAuthorById(string id, int page, int pageSize)
		{
			try
			{
				var posts = _spriderumContext.Posts.Where(x => x.CreatorId == id)
					.OrderByDescending(x => x.ViewsCount)
					.Select(x => new
					{
						Id = x.Id,
						Image = x.OgImageUrl,
						CategoryId = x.CatId,
						CategoryName = x.Cat.Name,
						Title = x.Title,
						NewTitle = x.NewTitle,
						Description = x.Description,
						CreatedAt = string.Format("{0:dd MMM,yyyy}", x.CreatedAt),
						CreatorName = x.Creator.DisplayName,
						CreatorId = x.CreatorId,
						ViewsCount = x.ViewsCount
					})
					.ToList();
				var totalPosts = posts.Count;

				if (totalPosts == 0)
				{
					return NotFound("Does not have any Post for this Author");
				}

				var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

				var pagedPosts = posts.Skip((page - 1) * pageSize).Take(pageSize);

				return Ok(new { Posts = pagedPosts, TotalPosts = totalPosts, TotalPages = totalPages });
			}
			catch (Exception ex)
			{
				return Conflict(ex.Message);
			}
		}
	}
}
