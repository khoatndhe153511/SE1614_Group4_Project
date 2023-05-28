using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class DatumController : Controller
    {
        private readonly IRepository<Datum> _datumRepository;

        public DatumController(IRepository<Datum> datumRepository)
        {
            _datumRepository = datumRepository;
        }

        [HttpGet]
        public IActionResult GetAllDatum()
        {
            return Ok(_datumRepository.GetAll());
        }

        [HttpGet]
        [Route("api/[controller]/[action]/{did}")]
        public IActionResult GetDatumById(int id)
        {
            try
            {
                return Ok(_datumRepository.Find(id));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpGet]
        public IActionResult UpdateDatum(Datum datum)
        {
            try
            {
                return Ok(_datumRepository.Update(datum));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpPost]
        public IActionResult AddDatum(Datum datum)
        {
            try
            {
                return Ok(_datumRepository.Add(datum));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpDelete]
        [Route("api/[controller]/[action]/{did}")]
        public IActionResult DeleteDatum(int id)
        {
            try
            {
                Datum? _ = _datumRepository.Find(id);
                if (_ != null)
                {
                    _datumRepository.Delete(_);
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
