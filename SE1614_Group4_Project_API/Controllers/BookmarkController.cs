using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;

namespace SE1614_Group4_Project_API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class BookmarkController : Controller
{
    private readonly spriderumContext _context;
    private readonly IMapper _mapper;

    public BookmarkController(spriderumContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetBookmarksByUserId(string userId, int page, int pageSize)
    {
        try
        {
            var result = _context.Bookmarks
                .Include(x => x.Post)
                .Where(x => x.UserId.Equals(userId))
                .Select(x => new
                {
                    Id = x.PostId,
                    Image = x.Post.OgImageUrl,
                    CategoryId = x.Post.CatId,
                    CategoryName = x.Post.Cat.Name,
                    Title = x.Post.Title,
                    Description = x.Post.Description,
                    CreatedAt = $"{x.Post.CreatedAt:dd MMM,yyyy}",
                    CreatorName = x.Post.Creator.DisplayName,
                    CreatorId = x.Post.CreatorId,
                    ViewsCount = x.Post.ViewsCount
                })
                .ToList();

            var totalPosts = result.Count;
            var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
            var pagedPosts = result.Skip((page - 1) * pageSize).Take(pageSize);

            return Ok(new { Posts = pagedPosts, TotalPosts = totalPosts, TotalPages = totalPages });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public IActionResult AddBookmark([FromBody] BookmarkDTO model)
    {
        try
        {
            if (model.PostId == null || model.UserId == null) return BadRequest("field cannot be null");

            var existedBookmark = _context.Bookmarks
                .FirstOrDefault(x => x.UserId.Equals(model.UserId) && x.PostId == model.PostId);
            if (existedBookmark != null) return BadRequest("Bookmark already have");

            _context.Bookmarks.Add(_mapper.Map<Bookmark>(model));
            var result = _context.SaveChanges();

            return Ok(result);
        }
        catch (Exception e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPost]
    public IActionResult RemoveBookmark([FromBody] BookmarkDTO model)
    {
        try
        {
            if (model.PostId == null || model.UserId == null) return BadRequest("field cannot be null");

            var existedBookmark = _context.Bookmarks
                .FirstOrDefault(x => x.UserId.Equals(model.UserId) && x.PostId == model.PostId);
            if (existedBookmark == null) return BadRequest("Bookmark does not exist");

            _context.Bookmarks.Remove(existedBookmark);
            _context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public bool CheckBookmarkExist(string userId, int postId)
    {
        var existedBookmark = _context.Bookmarks
            .FirstOrDefault(x => x.UserId.Equals(userId) && x.PostId == postId);
        return existedBookmark != null;
    }
}