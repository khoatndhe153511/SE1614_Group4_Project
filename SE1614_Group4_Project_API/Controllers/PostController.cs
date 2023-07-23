using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;
using System.Reflection.Metadata;

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
        public IActionResult GetAllPost(int page, int pageSize, string status)
        {
            var posts = _postRepository.GetPostsRecently();

            if (status.Equals("Accept"))
            {
                posts = posts.Where(_ => _.isEditorPick == true).ToList();
            }
            else if (status.Equals("Process"))
            {
                posts = posts.Where(_ => _.isEditorPick == null).ToList();
            }

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
                    .Where(x => x.IsEditorPick == true)
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Image = x.OgImageUrl,
                        Description = x.Description,
                        Created = $"{x.CreatedAt:dd MMM, yyyy}",
                        Modified = $"{x.ModifiedAt:dd MMM, yyyy}",
                        Title = x.Title,
                        CategoryId = x.CatId,
                        CategoryName = x.Cat.Name,
                        AuthorId = x.CreatorId,
                        AuthorName = x.Creator.Name,
                        ViewsCount = x.ViewsCount
                    }).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var totalPosts = posts.Count;
                var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

                return Ok(new { Posts = posts, TotalPosts = totalPosts, TotalPages = totalPages });
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
                    ogImageUrl = _.OgImageUrl,
                    created = $"{_.CreatedAt:dd MMM, yyyy}",
                    viewsCount = _.ViewsCount
                }).First();
                return Ok(post);
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetReadingPostById(int id)
        {
            try
            {
                var readingPost = _context.Posts.FirstOrDefault(p => p.Id == id && p.IsEditorPick == true);
                if (readingPost is null) return NotFound("Required post not found!");
                readingPost.ViewsCount++;
                _context.SaveChanges();

                var data = _postRepository.GetTextPost(id);
                var post = _context.Posts.Where(_ => _.Id == id).Select(_ => new
                {
                    id = _.Id,
                    postId = _.Id1,
                    title = _.Title,
                    isEditorPick = _.IsEditorPick,
                    categoryName = _.Cat.Name,
                    authorId = _.CreatorId,
                    authorName = _.Creator.DisplayName,
                    catId = _.CatId,
                    content = data,
                    slug = _.Slug,
                    description = _.Description,
                    ogImageUrl = _.OgImageUrl,
                    created = $"{_.CreatedAt:dd MMM, yyyy}",
                    viewsCount = _.ViewsCount
                }).First();
                return Ok(post);
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [HttpGet("{currentPostId:int}")]
        public IActionResult GetPrevAndNextPost(int currentPostId)
        {
            try
            {
                var previousPost = _context.Posts
                    .OrderByDescending(x => x.Id)
                    .Where(x => x.Id < currentPostId && x.IsEditorPick == true)
                    .Select(p => new
                    {
                        Id = p.Id,
                        Image = p.OgImageUrl,
                        Title = p.Title
                    })
                    .FirstOrDefault();

                var nextPost = _context.Posts
                    .OrderBy(x => x.Id)
                    .Where(x => x.Id > currentPostId && x.IsEditorPick == true)
                    .Select(p => new
                    {
                        Id = p.Id,
                        Image = p.OgImageUrl,
                        Title = p.Title
                    })
                    .FirstOrDefault();

                return Ok(new { previousPost, nextPost });
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet("{cateId:int}")]
        public ActionResult GetPostsByCate(int cateId, int page, int pageSize)
        {
            try
            {
                var posts = _context.Posts.Where(x => x.CatId == cateId && x.IsEditorPick == true)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Image = x.OgImageUrl,
                        CategoryId = x.CatId,
                        CategoryName = x.Cat.Name,
                        Title = x.Title,
                        NewTitle = x.NewTitle,
                        Description = x.Description,
                        CreatedAt = string.Format("{0:dd MMM, yyyy}", x.CreatedAt),
                        CreatorName = x.Creator.DisplayName,
                        CreatorId = x.CreatorId,
                        ViewsCount = x.ViewsCount
                    })
                    .ToList();
                var totalPosts = posts.Count;

                if (totalPosts == 0)
                {
                    return Ok("Does not have any Post for this Category");
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
                var result = _context.Posts
                    .Where(x => x.IsEditorPick == true)
                    .OrderByDescending(x => x.ViewsCount).Take(3)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Image = x.OgImageUrl,
                        CategoryId = x.CatId,
                        CategoryName = x.Cat.Name,
                        Title = x.Title,
                        NewTitle = x.NewTitle,
                        Description = x.Description,
                        CreatedAt = $"{x.CreatedAt:dd MMM, yyyy}",
                        CreatorName = x.Creator.DisplayName,
                        CreatorId = x.CreatorId,
                        ViewsCount = x.ViewsCount
                    }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public ActionResult UpdatePost([FromBody] UpdatePostDTO post)
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
        [Authorize(Roles = "2")]
        public ActionResult GetAllPostByUserId(string userId, int page, int pageSize, string status)
        {
            try
            {
                var posts = _postRepository.GetAllPostByUserId(userId);

                if (status.Equals("Accept"))
                {
                    posts = posts.Where(_ => _.isEditorPick == true).ToList();
                }
                else if (status.Equals("Denied"))
                {
                    posts = posts.Where(_ => _.isEditorPick == false).ToList();
                }
                else if (status.Equals("Process"))
                {
                    posts = posts.Where(_ => _.isEditorPick == null).ToList();
                }

                var totalPosts = posts.Count;
                var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

                var pagedPosts = posts.Skip((page - 1) * pageSize).Take(pageSize);

                return Ok(new { Posts = pagedPosts, TotalPosts = totalPosts, TotalPages = totalPages });
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
                    .Where(x => x.Title.ToLower().Contains(title.ToLower()) && x.IsEditorPick == true)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Image = x.OgImageUrl,
                        CategoryId = x.CatId,
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
        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "1")]
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

        [HttpGet]
        public IActionResult GetRates(int postId)
        {
            try
            {
                return Ok(_postRepository.GetRate(postId));
            }
            catch (Exception e)
            {
                return Conflict(e);
            }
        }

        [HttpGet]
        public bool? GetRatesbyUserId(int postId, string userId)
        {
            return _postRepository.GetRatesbyUserId(postId, userId);
        }

        [HttpGet]
        public IActionResult UpdateRates(int postId, string userId, bool? like)
        {
            try
            {
                _postRepository.UpdateRate(postId, userId, like);
                return Ok();
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }
    }
}