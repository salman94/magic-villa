using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers;

[Route("api/VillaApi")]
[ApiController]
public class VillaApiController: ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<VillaDto>> GetVillas()
    {
        return Ok(VillaStore.listVilla);
    }
    
    [HttpGet("{id:long}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<VillaDto?> GetVilla(long id)
    
    {
        if (id == 0)
        {
            return BadRequest();
        }
        var villaDto = VillaStore.listVilla.FirstOrDefault(u => u.Id == id);
        if (villaDto == null)
        {
            return NotFound();
        }

        return villaDto;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<VillaDto?> PostVilla([FromBody] VillaDto req)
    {
        if (req == null)
        {
            return BadRequest();
        }
        if (req.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (VillaStore.listVilla.FirstOrDefault(u => u.Name == req.Name) != null)
        {
            ModelState.AddModelError("CustomError", "Villa name should be unique");
            return BadRequest(ModelState);
        }
        var villaId = VillaStore.listVilla.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
        var villaDto = new VillaDto();
        villaDto.Id = villaId;
        villaDto.Name = req.Name;
        
        VillaStore.listVilla.Add(villaDto);

        return CreatedAtRoute("GetVilla", new {id = villaDto.Id}, villaDto);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteVilla(long id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villaDto = VillaStore.listVilla.FirstOrDefault(u => u.Id == id);
        if (villaDto == null)
        {
            return NotFound();
        }

        VillaStore.listVilla.Remove(villaDto);
        return NoContent();;
    }
    
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateVilla(long id, [FromBody] VillaDto req)
    {
        if (req == null || id == 0 || id != req.Id)
        {
            return BadRequest();
        }

        if (VillaStore.listVilla.FirstOrDefault(u => u.Name == req.Name) != null)
        {
            ModelState.AddModelError("CustomError", "Villa name should be unique");
            return BadRequest(ModelState);
        }
        var villaDto = VillaStore.listVilla.FirstOrDefault(u => u.Id == id);
        if (villaDto == null)
        {
            return NotFound();
        }
        villaDto.Name = req.Name;
        villaDto.Occupancy = req.Occupancy;
        villaDto.Sqft = req.Sqft;
        
        return NoContent();
    }
    
    [HttpPatch("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdatePartialVilla(long id, [FromBody] JsonPatchDocument<VillaDto> villaDocument)
    {
        if (villaDocument == null || id <= 0)
        {
            return BadRequest();
        }

        var villaDto = VillaStore.listVilla.FirstOrDefault(u => u.Id == id);
        if (villaDto == null)
        {
            return NotFound();
        }
        
        villaDocument.ApplyTo(villaDto, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        return NoContent();
    }
}