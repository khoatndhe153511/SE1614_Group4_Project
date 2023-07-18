using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;
using SE1614_Group4_Project_API.Utils;

namespace SE1614_Group4_Project_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly spriderumContext _spriderumContext;

        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository, spriderumContext spriderumContext)
        {
            _commentRepository = commentRepository;
            _spriderumContext = spriderumContext;
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

        [HttpPut("{id}")]
        [Authorize(Roles = "0,1,2,3")]
        public IActionResult UpdateComment(int id, CommentDTO commentDTO)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity is not null)
                {
                    var userClaims = identity.Claims;
                    var userId = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;

                    Comment? _ = _spriderumContext.Comments.FirstOrDefault(x => x.Id == id);

                    if (_ != null)
                    {
                        if (_.UserId == userId)
                        {
                            _.Content = commentDTO.Content;
                            _.ReplyUserId = commentDTO.ReplyUserId;
                            _spriderumContext.Comments.Update(_);
                            _spriderumContext.SaveChanges();
                        }
                        else return BadRequest("Can't Delete");
                    }

                    return Ok();
                }

                return BadRequest("Login!!");
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpPost]
        [Authorize(Roles = "0,1,2,3")]
        public IActionResult AddComment(CommentDTO commentDTO)
        {
            try
            {
                Comment comment = new Comment();
                comment.PostId = commentDTO.PostId;
                comment.Content = commentDTO.Content;
                comment.CreatedDate = DateTime.Now;
                comment.UserId = commentDTO.UserId;
                comment.ReplyUserId = commentDTO?.ReplyUserId;
                var lastComment = _spriderumContext.Comments.OrderBy(x => x.Id).LastOrDefault().Id;
                comment.Id = lastComment + 1;

                _spriderumContext.Comments.Add(comment);
                _spriderumContext.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "0,1,2,3")]
        public IActionResult DeleteComment(int id)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity is not null)
                {
                    var userClaims = identity.Claims;
                    var userId = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;

                    Comment? _ = _spriderumContext.Comments.FirstOrDefault(x => x.Id == id);

                    if (_ != null)
                    {
                        if (_.UserId == userId)
                        {
                            _spriderumContext.Comments.Remove(_);
                            _spriderumContext.SaveChanges();
                        }
                        else return BadRequest("Can't Delete");
                    }

                    return Ok();
                }

                return BadRequest("Login!!");
            }
            catch (DbUpdateException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetAllCommentByPostId(int page, int pageSize, int postId)
        {
            var query = _spriderumContext.Comments.Include(x => x.Post).Include(x => x.User)
                .Where(x => x.PostId == postId)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new CommentDTO
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    PostId = x.PostId,
                    UserName = x.User.DisplayName,
                    Content = x.Content,
                    CreatedDate = asTimeAgo(x.CreatedDate),
                    ReplyUserId = x.ReplyUserId,
                    imageUser = x.User.Avatar,
                });

            var totalCount = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var currentPage = Math.Min(page, totalPages);

            var users = await query.Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PageResult<CommentDTO>
            {
                TotalPage = totalPages,
                TotalCount = totalCount,
                Page = currentPage,
                PageSize = pageSize,
                Results = users.Select(x => new CommentDTO
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    PostId = x.PostId,
                    UserName = x.UserName,
                    Content = x.Content,
                    CreatedDate = x.CreatedDate,
                    ReplyUserId = x.ReplyUserId,
                    imageUser = x.imageUser,
                })
            };

            return Ok(result);
        }
        
        private static string asTimeAgo(DateTime date)
        {
            TimeSpan timeSpan = DateTime.Now.Subtract(date);

            return timeSpan.TotalSeconds switch
            {
                <= 60 => $"{timeSpan.Seconds} seconds ago",

                _ => timeSpan.TotalMinutes switch
                {
                    <= 1 => "about a minute ago",
                    < 60 => $"about {timeSpan.Minutes} minutes ago",
                    _ => timeSpan.TotalHours switch
                    {
                        <= 1 => "about an hour ago",
                        < 24 => $"about {timeSpan.Hours} hours ago",
                        _ => timeSpan.TotalDays switch
                        {
                            <= 1 => "yesterday",
                            <= 30 => $"about {timeSpan.Days} days ago",

                            <= 60 => "about a month ago",
                            < 365 => $"about {timeSpan.Days / 30} months ago",

                            <= 365 * 2 => "about a year ago",
                            _ => $"about {timeSpan.Days / 365} years ago"
                        }
                    }
                }
            };
        }
    }
}