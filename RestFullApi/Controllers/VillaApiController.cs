using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestFullApi.Data;
using RestFullApi.Logging;
using RestFullApi.Models;
using RestFullApi.Models.dto;

namespace RestFullApi.Controllers
{
    //[Route("/api/[controller]")] указали путь к контроллеру 
    [Route("/api/VillaApi")] // 2 пример как можно указать путь
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly ILogging _logger;

        public VillaApiController(ILogging logger)
        {
            _logger  = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.Log("Getting all villas","");
            return Ok(VillaDate.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[producesresponsetype(200, type = typeof(villadto))]
        //[producesresponsetype(404)]
        //[producesresponsetype(400)]
        public ActionResult<VillaDto> GetVillas(int id)
        {
            if (id == 0)
            {
                _logger.Log("error with id in:" + id, "error");
                return BadRequest();
            }
            var villa = VillaDate.villaList.FirstOrDefault(u => u.Id == id); // cтворив змінну яка показує по id

            if (villa == null)
            {
                return NotFound();
            }
            return Ok();

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villa)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (VillaDate.villaList.FirstOrDefault(u => u.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError ", "Villa already exists");
                return BadRequest(ModelState);
            }
            if (villa == null)
            {
                return BadRequest();
            }
            if (villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villa.Id = VillaDate.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaDate.villaList.Add(villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaDate.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaDate.villaList.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto updatedVilla)
        {
            if (updatedVilla == null || id != updatedVilla.Id)
            {
                return BadRequest();
            }
            var villa = VillaDate.villaList.FirstOrDefault(u => u.Id == id);
            villa.Name = updatedVilla.Name;
            villa.Sqft = updatedVilla.Sqft;
            villa.Occupancy = updatedVilla.Occupancy;

            return NoContent();
        }

        [HttpPatch("{id:int}", Name ="UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto) 
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaDate.villaList.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent() ;
        }
    }
}
