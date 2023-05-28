using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class BlockController : Controller
    {
        private readonly IRepository<Block> _blockRepository;

        public BlockController(Repository<Block> blockRepository)
        {
            _blockRepository = blockRepository;
        }

        [HttpGet]
        public IActionResult GetAllBlock()
        {
            return Ok(_blockRepository.GetAll());
        }

        [HttpGet]
        [Route("api/[controller]/[action]/{bid}")]
        public IActionResult GetBlockById(int id)
        {
            try
            {
                return Ok(_blockRepository.Find(id));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpGet]
        public IActionResult UpdateBlock(Block block)
        {
            try
            {
                return Ok(_blockRepository.Update(block));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpPost]
        public IActionResult AddBlock(Block block)
        {
            try
            {
                return Ok(_blockRepository.Add(block));
            }
            catch (Exception e)
            {
                return Conflict();
            }
        }

        [HttpDelete]
        [Route("api/[controller]/[action]/{bid}")]
        public IActionResult DeleteBlock(int id)
        {
            try
            {
                Block? _ = _blockRepository.Find(id);
                if (_ != null)
                {
                    _blockRepository.Delete(_);
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
