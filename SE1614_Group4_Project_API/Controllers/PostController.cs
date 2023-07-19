using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;
using SE1614_Group4_Project_API.DTOs;
using Microsoft.Extensions.Hosting;

namespace SE1614_Group4_Project_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly spriderumContext _context;

        public PostController(IPostRepository postRepository, spriderumContext context)
        {
            _postRepository = postRepository;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllPost(int page, int pageSize)
        {
            var posts = _context.Posts.OrderByDescending(_ => _.CreatedAt).Select(_ => new
            {
                id = _.Id,
                Image = _.OgImageUrl,
                Description = _.Description,
                Created = $"{_.CreatedAt:dd MMM,yyyy}",
                Modified = $"{_.ModifiedAt:dd MMM,yyyy}",
                isEditorPick = _.IsEditorPick,
                title = _.Title,
                CategoryName = _.Cat.Name,
                AuthorId = _.CreatorId,
                AuthorName = _.Creator.Name,
                ViewsCount = _.ViewsCount           
            }).ToList();

            var totalPosts = posts.Count;
            var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

            var pagedPosts = posts.Skip((page - 1) * pageSize).Take(pageSize);

            return Ok(new { Posts = pagedPosts, TotalPosts = totalPosts, TotalPages = totalPages });
        }

        [HttpGet]
        public IActionResult GetRecentPosts(int page, int pageSize)
        {
            try
            {
                var posts = _context.Posts
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Image = x.OgImageUrl,
                        Description = x.Description,
                        Created = $"{x.CreatedAt:dd MMM,yyyy}",
                        Modified = $"{x.ModifiedAt:dd MMM,yyyy}",
                        Title = x.Title,
                        CategoryName = x.Cat.Name,
                        AuthorId = x.CreatorId,
                        AuthorName = x.Creator.Name,
                        ViewsCount = x.ViewsCount
                    }).ToList();

                var totalPosts = posts.Count;
                var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

                var pagedPosts = posts.Skip((page - 1) * pageSize).Take(pageSize);

                return Ok(new { Posts = pagedPosts, TotalPosts = totalPosts, TotalPages = totalPages });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(int id)
        {
            try
            {
                 var data = _postRepository.GetTextPost(id);
                 var post = _context.Posts.Where(_ => _.Id == id).Select(_ => new
                {
                    id = _.Id,
                    postId = _.Id1,
                    title = _.Title,
                    isEditorPick = _.IsEditorPick,
                    categoryName = _.Cat.Name,
                    authorId = _.CreatorId,
                    authorName = _.Creator.Name,
                    catId = _.CatId,
                    content = data,
                    slug = _.Slug,
                    description = _.Description,
                    ogImageUrl = _.OgImageUrl
                }).First();
                return Ok(post);
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [HttpGet("{cateId:int}")]
        public ActionResult GetPostsByCate(int cateId, int page, int pageSize)
        {
            try
            {
                var posts = _context.Posts.Where(x => x.CatId == cateId)
                    .Select(x => new
                    {
                        Image = x.OgImageUrl,
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
                    return NotFound("Does not have any Post for this Category");
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

        [HttpGet]
        public ActionResult GetPopularPosts()
        {
            try
            {
                var result = _context.Posts.Select(x => new
                {
                    Image = x.OgImageUrl,
                    CategoryName = x.Cat.Name,
                    Title = x.Title,
                    NewTitle = x.NewTitle,
                    Description = x.Description,
                    CreatedAt = $"{x.CreatedAt:dd MMM,yyyy}",
                    CreatorName = x.Creator.DisplayName,
                    CreatorId = x.CreatorId,
                    ViewsCount = x.ViewsCount
                }).OrderByDescending(x => x.ViewsCount).Take(3).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdatePost([FromBody] UpdatePostDTO post)
        {
            try
            {
                _postRepository.UpdatePostRecently(post);
                return Ok();
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpGet]
        public ActionResult SearchPostByName(string title, int page, int pageSize)
        {
            try
            {
                var posts = _context.Posts
                    .Where(x => x.Title.ToLower().Contains(title.ToLower()))
                    .Select(x => new
                    {
                        Image = x.OgImageUrl,
                        CategoryName = x.Cat.Name,
                        Title = x.Title,
                        NewTitle = x.NewTitle,
                        Description = x.Description,
                        CreatedAt = $"{x.CreatedAt:dd MMM,yyyy}",
                        CreatorName = x.Creator.DisplayName,
                        CreatorId = x.CreatorId,
                        ViewsCount = x.ViewsCount
                    })
                    .ToList();
                var totalPosts = posts.Count;

                if (totalPosts == 0)
                {
                    return NotFound("Does not have any Post for this Category");
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

        [HttpPost]
        public IActionResult UpdateStatus([FromBody] UpdateStatusDTO status)
        {
            try
            {
                _postRepository.UpdateStatus(status);
                return Ok();
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpPost]
        public IActionResult AddPost([FromBody] AddPostDTO post)
        {
            try
            {
                _postRepository.AddPostRecently(post);
                return Ok();
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