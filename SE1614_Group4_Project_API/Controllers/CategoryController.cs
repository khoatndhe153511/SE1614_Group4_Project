using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _catRepository;

        public CategoryController(ICategoryRepository catRepository)
        {
            _catRepository = catRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCat()
        {
            var categories = await _catRepository.GetAll();
            return Ok(categories);
        }

        [HttpGet]
        public ActionResult GetTop5Category()
        {
            var categories = _catRepository.GetTop5Category();
            if (categories.Count == 0) return BadRequest(new { errMess = "Does not have any categories in list" });

            return Ok(categories);
        }

        [HttpGet("{cid}")]
        public IActionResult GetCatById(int id)
        {
            try
            {
                return Ok(_catRepository.Find(id));
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [Authorize(Roles = "0, 1, 2")]
        [HttpPut]
        public IActionResult UpdateCat(Cat cat)
        {
            try
            {
                return Ok(_catRepository.Update(cat));
            }
            catch (Exception)
            {
                return Conflict();
            }
        }


        [Authorize(Roles = "0, 1, 2")]
        [HttpPost]
        public IActionResult AddCat(Cat cat)
        {
            try
            {
                return Ok(_catRepository.Add(cat));
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [Authorize(Roles = "0, 1, 2")]
        [HttpDelete("{cid}")]
        public IActionResult DeleteCat(int id)
        {
            try
            {
                Cat? _cat = _catRepository.Find(id);
                if (_cat != null)
                {
                    _catRepository.Delete(_cat);
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}