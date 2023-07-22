using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;
using SE1614_Group4_Project_API.Utils;
using SE1614_Group4_Project_API.Utils.Interfaces;
using System.Security.Claims;
using System.Security.Principal;

namespace SE1614_Group4_Project_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ILogicHandler _logicHandler;
        private readonly spriderumContext _spriderumContext;
        private readonly IWebHostEnvironment _env;

        public UserController(IUserRepository userRepository, IPostRepository postRepository,
            ILogicHandler logicHandler, spriderumContext spriderumContext, IWebHostEnvironment env)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _logicHandler = logicHandler;
            _spriderumContext = spriderumContext;
            _env = env;
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            try
            {
                return Ok(_userRepository.GetAll().Result);
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpGet("{uid}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                return Ok(_userRepository.Find(id));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpGet]
        public IActionResult UpdateUser(User user)
        {
            try
            {
                return Ok(_userRepository.Update(user));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            try
            {
                return Ok(_userRepository.Add(user));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpDelete("{uid}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                User? _ = _userRepository.Find(id);
                if (_ != null)
                {
                    _userRepository.Delete(_);
                }

                return Ok();
            }
            catch (DbUpdateException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword([FromBody] ChangePasswordModelDto model)
        {
            var currentUser = GetCurrentUser();
            if (currentUser is null)
            {
                return NotFound(new { errMess = Constants.ERR003 });
            }

            var user = _userRepository.findByName(currentUser.Name);
            if (user is null)
            {
                return NotFound(new { errMess = Constants.ERR002 });
            }

            if (user.Password != model.OldPassword)
            {
                return BadRequest(new { errMess = "Your old password is incorrect" });
            }
            else if (model.OldPassword == model.NewPassword)
            {
                return BadRequest(new { errMess = "You cannot set your new password same like old password!" });
            }
            else if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest(new
                {
                    errMess = "Confirm password failed!"
                });
            }

            try
            {
                user.Password = model.NewPassword;
                _userRepository.UpdatePassword(user.Email, model.NewPassword);
                Response.Cookies.Delete("ACCESS_TOKEN");
                return Ok(new
                {
                    successMess = "Password changed successfully!"
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var user = _userRepository.FindByEmail(model.Email);
            if (user is null)
            {
                return NotFound(Constants.ERR002);
            }

            string tempPass = _logicHandler.GeneratePassword(12);

            _logicHandler.SendEmailAsync(model.Email, "[Spriderum] Reset Password",
                $"Your new password is: {tempPass}.");

            _userRepository.UpdatePassword(model.Email, tempPass);
            return Ok("Password Reset Successfully!");
        }

        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity is not null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Role = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value)
                };
            }

            return null;
        }

        [HttpGet]
        [Authorize(Roles = "0, 1, 2, 3")]
        public IActionResult GetDetailProfile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity is not null)
            {
                var userClaims = identity.Claims;
                var id = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;

                var userProfile = _userRepository.findById(id);

                var _totalPost = _postRepository.CountTotalPostByUserId(userProfile.Id);
                var _totalComment = _postRepository.CountTotalCommentByUserId(userProfile.Id);
                var _totalPoint = _postRepository.TotalPointByUserId(userProfile.Id);
                var _totalView = _postRepository.CountTotalViewByUserId(userProfile.Id);
                return Ok(new
                {
                    username = userProfile.Name,
                    avatar = userProfile.Avatar,
                    displayName = userProfile.DisplayName,
                    grAvatar = userProfile.Gravatar,
                    phoneNumber = userProfile.PhoneNumber,
                    email = userProfile.Email,
                    birth = userProfile.Birth,
                    gender = userProfile.Gender,
                    totalPost = _totalPost,
                    totalView = _totalView,
                    totalComment = _totalComment,
                    totalPoint = _totalPoint,
                });
            }

            return null;
        }

        [HttpGet]
        [Authorize(Roles = "0, 1, 2, 3")]
        public async Task<IActionResult> GetListPostOfUser(int page, int pageSize)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity is not null)
            {
                var userClaims = identity.Claims;
                var id = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;

                var userProfile = _userRepository.findById(id);

                var query = _spriderumContext.Posts.Where(x => x.CreatorId.Equals(userProfile.Id))
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new Post
                    {
                        Id = x.Id,
                        Id1 = x.Id1,
                        CommentCount = x.CommentCount,
                        CatId = x.CatId,
                        ControlversialPoint = x.ControlversialPoint,
                        DatePoint = x.DatePoint,
                        Description = x.Description,
                        CreatedAt = x.CreatedAt,
                        HotPoint = x.HotPoint,
                        NewTitle = x.NewTitle,
                        OgImageUrl = x.OgImageUrl,
                        Point = x.Point,
                        Slug = x.Slug,
                        Title = x.Title,
                        ViewsCount = x.ViewsCount,
                        Thumbnail = x.Thumbnail
                    });

                if (query == null)
                {
                    return NotFound();
                }

                var totalCount = await query.CountAsync();

                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                var currentPage = Math.Min(page, totalPages);

                var posts = await query.Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var result = new PageResult<Post>
                {
                    TotalPage = totalPages,
                    TotalCount = totalCount,
                    Page = currentPage,
                    PageSize = pageSize,
                    Results = posts.Select(x => new Post
                    {
                        Id = x.Id,
                        Id1 = x.Id1,
                        CommentCount = x.CommentCount,
                        CatId = x.CatId,
                        ControlversialPoint = x.ControlversialPoint,
                        DatePoint = x.DatePoint,
                        Description = x.Description,
                        CreatedAt = x.CreatedAt,
                        HotPoint = x.HotPoint,
                        NewTitle = x.NewTitle,
                        OgImageUrl = x.OgImageUrl,
                        Point = x.Point,
                        Slug = x.Slug,
                        Title = x.Title,
                        ViewsCount = x.ViewsCount,
                        Thumbnail = x.Thumbnail
                    })
                };

                return Ok(result);
            }

            return null;
        }

        [HttpGet("{username}")]
        [Authorize(Roles = "0, 1, 2, 3")]
        public IActionResult CheckUsernameExist(string username)
        {
            var check = _userRepository.checkUsername(username);
            return Ok(check);
        }

        [HttpGet("{email}")]
        [Authorize(Roles = "0, 1, 2, 3")]
        public IActionResult CheckEmailExist(string email)
        {
            var check = _userRepository.checkEmail(email);
            return Ok(check);
        }

        [HttpPost]
        [Authorize(Roles = "0, 1, 2, 3")]
        public IActionResult ChangePasswordUser([FromBody] ChangePassDTO model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity is not null)
            {
                var userClaims = identity.Claims;
                var id = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
                var user = _userRepository.findById(id);
                var checkPass = checkCurrentPass(user, model.OldPassword);

                if (checkPass)
                {
                    user.Password = model.NewPassword;
                    _userRepository.UpdatePassword(user.Email, model.NewPassword);
                    return Ok(true);
                }
                else return Ok(false);
            }
            else return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "0, 1, 2, 3")]
        public IActionResult UpdateUserProfile([FromBody] UpdateUserProfile model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity is not null)
            {
                var userClaims = identity.Claims;
                var id = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
                var user = _userRepository.findById(id);
                return Ok(_userRepository.updateUserProfile(user, model));
            }
            else return BadRequest();
        }

        private bool checkCurrentPass(User user, string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            if (user != null)
            {
                if (user.Password == password) return true;
                else return false;
            }
            else return false;

            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize(Roles = "0,1,2,3")]
        public IActionResult uploadimage(IFormFile imageFile)
        {
            Random r = new Random();
            int random = r.Next();

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            try
            {
                if (identity is not null)
                {
                    var userClaims = identity.Claims;
                    var id = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
                    var user = _userRepository.findById(id);

                    // Kiểm tra xem tệp đã được chọn hay chưa
                    if (imageFile == null || imageFile.Length <= 0)
                    {
                        return BadRequest("Vui lòng chọn tệp ảnh để upload.");
                    }

                    string extension = Path.GetExtension(imageFile.FileName);

                    if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") ||
                        extension.ToLower().Equals(".png"))
                    {
                        // Đường dẫn tới thư mục bạn muốn lưu trữ tệp ảnh
                        string targetFolderPath =
                            "..\\..\\SE1614_Group4_Project\\SE1614_Group4_Project_FE\\tech-blog\\images";

                        // Tạo thư mục nếu nó không tồn tại
                        if (!Directory.Exists(targetFolderPath))
                        {
                            Directory.CreateDirectory(targetFolderPath);
                        }

                        string imageNameFile = random + imageFile.FileName;
                        // Tạo đường dẫn đầy đủ cho tệp ảnh mới
                        string newImagePath = Path.Combine(targetFolderPath, imageNameFile);

                        // Lưu tệp ảnh vào thư mục đã chỉ định
                        using (var stream = new FileStream(newImagePath, FileMode.Create))
                        {
                            imageFile.CopyTo(stream);
                        }

                        user.Avatar = imageNameFile;
                        _spriderumContext.Users.Update(user);
                        _spriderumContext.SaveChanges();
                        return Ok("Đã upload thành công ảnh.");
                    }
                    else
                    {
                        return BadRequest("file is not image");
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }
    }
}