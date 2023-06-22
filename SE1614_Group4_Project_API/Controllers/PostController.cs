using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository;
using SE1614_Group4_Project_API.Repository.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SE1614_Group4_Project_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class PostController : Controller
    {

        private readonly IPostRepository _postRepository;
        private readonly spriderumContext _spriderumContext;

        public PostController(IPostRepository postRepository,spriderumContext spriderumContext)
        {
            _spriderumContext = spriderumContext;
            _postRepository = postRepository;
        }

        [HttpGet]
        public IActionResult GetAllPost(int page, int pageSize)
        {
            var posts = _spriderumContext.Posts.Select(_ => new
            {
                id = _.Id,
                Created = _.CreatedAt,
                Modified = _.ModifiedAt,
                title = _.Title,
                CategoryName = _.Cat.Name,
                AuthorName = _.Creator.Name
            }).ToList();

            var totalPosts = posts.Count;
            var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

            var pagedPosts = posts.Skip((page - 1) * pageSize).Take(pageSize);

            return Ok(new { Posts = pagedPosts, TotalPosts = totalPosts, TotalPages = totalPages });
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(int id)
        {
            try
            {
                 var post = _spriderumContext.Posts.Where(_ => _.Id == id).Select(_ => new
                {
                    id = _.Id,
                    title = _.Title,
                    isEditorPick = _.IsEditorPick,
                    categoryName = _.Cat.Name,
                    authorName = _.Creator.Name,
                    slug = _.Slug,
                    description = _.Description,
                    ogImageUrl = _.OgImageUrl
                }).First();
                return Ok(post);
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