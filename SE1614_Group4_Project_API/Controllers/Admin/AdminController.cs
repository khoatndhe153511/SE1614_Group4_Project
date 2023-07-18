using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository;
using SE1614_Group4_Project_API.Repository.Interfaces;
using SE1614_Group4_Project_API.Utils;
using System.Security.Claims;

namespace SE1614_Group4_Project_API.Controllers.Admin
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AdminController : Controller

	{
		private readonly spriderumContext _spriderumContext;

		private readonly IUserRepository _userRepository;

		public AdminController(spriderumContext spriderumContext, IUserRepository userRepository)
		{
			_spriderumContext = spriderumContext;
			_userRepository = userRepository;
		}


		[HttpPost]
		[Authorize(Roles = "0")]
		public IActionResult updateRoleAndActive([FromBody] ChangeRoleDTO changeRoleDTO)
		{
			var user = _userRepository.updateRoleAndActive(changeRoleDTO._id, changeRoleDTO.role, changeRoleDTO.active);
			return Ok(user);
		}


		[HttpGet]
		[Authorize(Roles = "0")]
		public async Task<IActionResult> GetListUser(int page, int pageSize)
		{
			var query = _spriderumContext.Users.Include(x => x.Posts).Include(x => x.Comments)
				.OrderBy(x => x.DisplayName)
				.Select(x => new UserDTO
				{
					Id = x.Id,
					Avatar = x.Avatar,
					Name = x.Name,
					DisplayName = x.DisplayName,
					Role = x.Role,
					PhoneNumber = x.PhoneNumber,
					Email = x.Email,
					Birth = x.Birth,
					Gender = x.Gender,
					Active = x.Active,
					TotalPost = x.Posts.Count(),
					TotalComment = x.Comments.Count()

				});

			var totalCount = await query.CountAsync();

			var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
			var currentPage = Math.Min(page, totalPages);

			var users = await query.Skip((currentPage - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			var result = new PageResult<UserDTO>
			{
				TotalPage = totalPages,
				TotalCount = totalCount,
				Page = currentPage,
				PageSize = pageSize,
				Results = users.Select(x => new UserDTO
				{
					Id = x.Id,
					Avatar = x.Avatar,
					Name = x.Name,
					DisplayName = x.DisplayName,
					Role = x.Role,
					PhoneNumber = x.PhoneNumber,
					Email = x.Email,
					Birth = x.Birth,
					Gender = x.Gender,
					Active = x.Active,
					TotalPost = x.TotalPost,
					TotalComment = x.TotalComment
				})
			};

			return Ok(result);

		}

		[HttpGet]
		[Authorize(Roles = "0")]
		public async Task<IActionResult> GetTotalIndex()
		{
			var totalPost = _spriderumContext.Posts.Count();
			var totalComment = _spriderumContext.Comments.Count();
			var totalUser = _spriderumContext.Users.Count();
			var totalPoint = _spriderumContext.Posts.Sum(x => x.Point);
			var totalView = _spriderumContext.Posts.Sum(x => x.ViewsCount);


			return Ok(new
			{
				TotalComment = totalComment
				,
				TotalPost = totalPost
				,
				TotalUser = totalUser
				,
				TotalView = totalView
				,
				TotalPoint = totalPoint
			});

		}

		[HttpGet]
		[Authorize(Roles = "0")]
		public async Task<IActionResult> GetRole()
		{
			var admin = new RoleDTO { Id = 0, Name = "admin" };
			var editorial = new RoleDTO { Id = 1, Name = "editorial" };
			var writer = new RoleDTO { Id = 2, Name = "writer" };
			var user = new RoleDTO { Id = 3, Name = "user" };

			var roles = new List<RoleDTO>();
			roles.Add(admin);
			roles.Add(editorial);
			roles.Add(writer);
			roles.Add(user);
			return Ok(roles);

		}

        [HttpGet]
        [Authorize(Roles = "0")]
        public async Task<IActionResult> SearchUser(int page, int pageSize, string keyword)
        {
            var query = _spriderumContext.Users.Include(x => x.Posts).Include(x => x.Comments)
                 .Where(x => x.DisplayName.ToLower().Contains(keyword.ToLower()) ||
                                x.Id.ToLower().Contains(keyword.ToLower()) ||
                                x.Name.ToLower().Contains(keyword.ToLower()))
                .OrderBy(x => x.DisplayName)
                .Select(x => new UserDTO
                {
                    Id = x.Id,
                    Avatar = x.Avatar,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    Role = x.Role,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    Birth = x.Birth,
                    Gender = x.Gender,
                    Active = x.Active,
                    TotalPost = x.Posts.Count(),
                    TotalComment = x.Comments.Count()

                });

            var totalCount = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var currentPage = Math.Min(page, totalPages);

            var users = await query.Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PageResult<UserDTO>
            {
                TotalPage = totalPages,
                TotalCount = totalCount,
                Page = currentPage,
                PageSize = pageSize,
                Results = users.Select(x => new UserDTO
                {
                    Id = x.Id,
                    Avatar = x.Avatar,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    Role = x.Role,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    Birth = x.Birth,
                    Gender = x.Gender,
                    Active = x.Active,
                    TotalPost = x.TotalPost,
                    TotalComment = x.TotalComment
                })
            };

            return Ok(result);

        }

   }
}
