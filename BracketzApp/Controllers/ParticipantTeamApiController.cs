using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BracketzApp.Data;
using Newtonsoft.Json;

namespace BracketzApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantTeamApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private JsonSerializerSettings settings;

        public ParticipantTeamApiController(ApplicationDbContext context)
        {
            _context = context;

            settings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented, // Just for humans
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        // GET: api/ParticipantTeamApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipantTeam>>> GetParticipantTeam()
        {
            var participantTeams = await _context.ParticipantTeam.Include(t => t.Participant).ToListAsync();
            return Content(JsonConvert.SerializeObject(participantTeams, settings), "application/json");
        }

        // GET: api/ParticipantTeamApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipantTeam>> GetParticipantTeam(int id)
        {
            var participantTeam = await _context.ParticipantTeam.Include(t => t.Participant).FirstOrDefaultAsync(x => x.TeamId == id);

            if (participantTeam == null)
            {
                return NotFound();
            }

            return Content(JsonConvert.SerializeObject(participantTeam, settings), "application/json");
        }

        // PUT: api/ParticipantTeamApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipantTeam(int id, ParticipantTeam participantTeam)
        {
            if (id != participantTeam.ParticipantId)
            {
                return BadRequest();
            }

            _context.Entry(participantTeam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantTeamExists(id))
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

        // POST: api/ParticipantTeamApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParticipantTeam>> PostParticipantTeam(ParticipantTeam participantTeam)
        {
            _context.ParticipantTeam.Add(participantTeam);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ParticipantTeamExists(participantTeam.ParticipantId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetParticipantTeam", new { id = participantTeam.ParticipantId }, participantTeam);
        }

        // DELETE: api/ParticipantTeamApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipantTeam(int id)
        {
            var participantTeam = await _context.ParticipantTeam.FindAsync(id);
            if (participantTeam == null)
            {
                return NotFound();
            }

            _context.ParticipantTeam.Remove(participantTeam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParticipantTeamExists(int id)
        {
            return _context.ParticipantTeam.Any(e => e.ParticipantId == id);
        }
    }
}
