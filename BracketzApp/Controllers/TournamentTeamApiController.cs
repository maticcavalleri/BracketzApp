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
    public class TournamentTeamApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private JsonSerializerSettings settings;

        public TournamentTeamApiController(ApplicationDbContext context)
        {
            _context = context;

            settings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented, // Just for humans
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        // GET: api/TournamentTeamApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentTeam>>> GetTournamentTeam()
        {
            var tournamentTeams = await _context.TournamentTeam.Include(t => t.Team).ToListAsync();
            return Content(JsonConvert.SerializeObject(tournamentTeams, settings), "application/json");
        }

        // GET: api/TournamentTeamApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentTeam>> GetTournamentTeam(int id)
        {
            var tournamentTeam = await _context.TournamentTeam.Include(t => t.Team).FirstOrDefaultAsync(x => x.TournamentId == id);


            if (tournamentTeam == null)
            {
                return NotFound();
            }

            return Content(JsonConvert.SerializeObject(tournamentTeam, settings), "application/json");
        }

        // PUT: api/TournamentTeamApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentTeam(int id, TournamentTeam tournamentTeam)
        {
            if (id != tournamentTeam.TournamentId)
            {
                return BadRequest();
            }

            _context.Entry(tournamentTeam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TournamentTeamExists(id))
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

        // POST: api/TournamentTeamApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentTeam>> PostTournamentTeam(TournamentTeam tournamentTeam)
        {
            _context.TournamentTeam.Add(tournamentTeam);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TournamentTeamExists(tournamentTeam.TournamentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTournamentTeam", new { id = tournamentTeam.TournamentId }, tournamentTeam);
        }

        // DELETE: api/TournamentTeamApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentTeam(int id)
        {
            var tournamentTeam = await _context.TournamentTeam.FindAsync(id);
            if (tournamentTeam == null)
            {
                return NotFound();
            }

            _context.TournamentTeam.Remove(tournamentTeam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TournamentTeamExists(int id)
        {
            return _context.TournamentTeam.Any(e => e.TournamentId == id);
        }
    }
}
