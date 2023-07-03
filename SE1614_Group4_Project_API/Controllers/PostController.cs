using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;

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
            var posts = _context.Posts.Select(_ => new
            {
                id = _.Id,
                Image = _.OgImageUrl,
                Description = _.Description,
                Created = string.Format("{0:dd MMM,yyyy}", _.CreatedAt),
                Modified = string.Format("{0:dd MMM,yyyy}", _.ModifiedAt),
                title = _.Title,
                CategoryName = _.Cat.Name,
                AuthorName = _.Creator.Name,
                ViewsCount = _.ViewsCount
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
                var post = _context.Posts.Where(_ => _.Id == id).Select(_ => new
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
                var popularPosts = _postRepository.GetPopularPosts();
                if (popularPosts.Count == 0)
                {
                    return NotFound("Does not have Posts");
                }

                return Ok(popularPosts);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult SearchPostByName(string title)
        {
            try
            {
                var posts = _postRepository.SearchPosts(title);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
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