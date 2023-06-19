using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CommentController : Controller
	{
		private readonly IRepository<Comment> _commentRepository;

		public CommentController(IRepository<Comment> commentRepository)
		{
			_commentRepository = commentRepository;
		}

		[HttpGet]
		public IActionResult GetAllComment()
		{
			return Ok(_commentRepository.GetAll());
		}

		[HttpGet("{did}")]
		public IActionResult GetCommentById(int id)
		{
			try
			{
				return Ok(_commentRepository.Find(id));
			}
			catch (Exception e)
			{
				return Conflict();
			}
		}

		[HttpGet]
		public IActionResult UpdateComment(Comment comment)
		{
			try
			{
				return Ok(_commentRepository.Update(comment));
			}
			catch (Exception e)
			{
				return Conflict();
			}
		}

		[HttpPost]
		public IActionResult AddComment(Comment comment)
		{
			try
			{
				return Ok(_commentRepository.Add(comment));
			}
			catch (Exception e)
			{
				return Conflict();
			}
		}

		[HttpDelete("{did}")]
		public IActionResult DeleteComment(int id)
		{
			try
			{
				Comment? _ = _commentRepository.Find(id);
				if (_ != null)
				{
					_commentRepository.Delete(_);
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
