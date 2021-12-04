using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    public class RaceController : Controller
    {
        private readonly DataContext _context;

        public RaceController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Race>>> GetRaces()
        {
            return await _context.races.OrderBy(x => x.Description).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Race>> GetRace(int id)
        {
            Race race = await _context.races.FindAsync(id);

            if (race == null)
            {
                return NotFound();
            }

            return race;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRace(int id, Race race)
        {
            if (id != race.Id)
            {
                return BadRequest();
            }

            _context.Entry(race).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe esta raza.");
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
        public async Task<ActionResult<DocumentType>> PostRace(Race race)
        {
            _context.races.Add(race);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetRace", new { id = race.Id }, race);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe tipo de raza.");
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
        public async Task<IActionResult> DeleteRace(int id)
        {
            Race race = await _context.races.FindAsync(id);
            if (race == null)
            {
                return NotFound();
            }

            _context.races.Remove(race);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
