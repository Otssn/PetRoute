using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetRoute.API.Data;
using PetRoute.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Controllers.api
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PetTypeController : Controller
    {
        private readonly DataContext _context;

        public PetTypeController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetType>>> GetPetTypes()
        {
            return await _context.petTypes.OrderBy(x => x.Description).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PetType>> GetPetType(int id)
        {
            PetType petType = await _context.petTypes.FindAsync(id);

            if (petType == null)
            {
                return NotFound();
            }

            return petType;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPetType(int id, PetType petType)
        {
            if (id != petType.Id)
            {
                return BadRequest();
            }

            _context.Entry(petType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe este tipo de mascota.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PetType>> PostPetType(PetType petType)
        {
            _context.petTypes.Add(petType);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetPetType", new { id = petType.Id }, petType);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe tipo de mascota.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetType(int id)
        {
            PetType petType = await _context.petTypes.FindAsync(id);
            if (petType == null)
            {
                return NotFound();
            }

            _context.petTypes.Remove(petType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
