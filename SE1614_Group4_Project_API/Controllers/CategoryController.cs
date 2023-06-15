using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult GetAllCat()
        {
            return Ok(_catRepository.GetAll());
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

        [HttpGet]
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