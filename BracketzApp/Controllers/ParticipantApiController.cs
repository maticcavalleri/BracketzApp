using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BracketzApp.Data;
using BracketzApp.Models;

namespace BracketzApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParticipantApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ParticipantApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participant>>> GetParticipant()
        {
            return await _context.Participant.Include(t => t.User).ToListAsync();
        }

        // GET: api/ParticipantApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Participant>> GetParticipant(int id)
        {
            var participant = await _context.Participant.Include(t => t.User).FirstOrDefaultAsync(x => x.Id == id);

            if (participant == null)
            {
                return NotFound();
            }

            return participant;
        }

        // PUT: api/ParticipantApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipant(int id, Participant participant)
        {
            if (id != participant.Id)
            {
                return BadRequest();
            }

            _context.Entry(participant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ParticipantApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Participant>> PostParticipant(Participant participant)
        {
            _context.Participant.Add(participant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParticipant", new { id = participant.Id }, participant);
        }

        // DELETE: api/ParticipantApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant(int id)
        {
            var participant = await _context.Participant.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            _context.Participant.Remove(participant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParticipantExists(int id)
        {
            return _context.Participant.Any(e => e.Id == id);
        }
    }
}
