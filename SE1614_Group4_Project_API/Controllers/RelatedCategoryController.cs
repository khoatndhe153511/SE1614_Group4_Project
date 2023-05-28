using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class RelatedCategoryController : Controller
    {
        private readonly IRepository<RelatedCat> _catRepository;

        public RelatedCategoryController(IRepository<RelatedCat> catRepository)
        {
            _catRepository = catRepository;
        }

        [HttpGet]
        public IActionResult GetAllCat()
        {
            return Ok(_catRepository.GetAll());
        }

        [HttpGet]
        [Route("api/[controller]/[action]/{pid}")]
        public IActionResult GetRelatedCatById(int id)
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
        public IActionResult UpdateRelatedCat(RelatedCat cat)
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
        public IActionResult AddRelatedCat(RelatedCat cat)
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

        [HttpDelete]
        [Route("api/[controller]/[action]/{pid}")]
        public IActionResult DeleteRelatedCat(int id)
        {
            try
            {
                RelatedCat? _ = _catRepository.Find(id);
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
