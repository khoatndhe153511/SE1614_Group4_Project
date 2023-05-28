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
        private readonly IRepository<Cat> _catRepository;

        public CategoryController(IRepository<Cat> catRepository)
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpDelete("{cid}")]
        public IActionResult DeleteCat(int id)
        {
            try
            {
                Cat? _ = _catRepository.Find(id);
                if (_ != null)
                {
                    _catRepository.Delete(_);
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
