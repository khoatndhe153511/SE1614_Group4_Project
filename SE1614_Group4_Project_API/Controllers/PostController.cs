using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class PostController : Controller
    {

		private readonly IPostRepository _postRepository;

		public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        public IActionResult GetAllPost()
        {
            return Ok(_postRepository.GetAll());
        }

        [HttpGet("{pid}")]
        public IActionResult GetPostById(int id)
        {
            try
            {
                return Ok(_postRepository.Find(id));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpGet]
        public IActionResult UpdatePost(Post post)
        {
            try
            {
                return Ok(_postRepository.Update(post));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpPost]
        public IActionResult AddPost(Post post)
        {
            try
            {
                return Ok(_postRepository.Add(post));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpDelete("{pid}")]
        public IActionResult DeletePost(int id)
        {
            try
            {
                Post? _ = _postRepository.Find(id);
                if (_ != null)
                {
                    _postRepository.Delete(_);
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