using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BracketzApp.Data;

namespace BracketzApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BracketTeamApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BracketTeamApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BracketTeamControllerApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BracketTeam>>> GetBracketTeam()
        {
            return await _context.BracketTeam.ToListAsync();
        }

        // GET: api/BracketTeamControllerApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BracketTeam>> GetBracketTeam(int id)
        {
            var bracketTeam = await _context.BracketTeam.FindAsync(id);

            if (bracketTeam == null)
            {
                return NotFound();
            }

            return bracketTeam;
        }

        // PUT: api/BracketTeamControllerApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBracketTeam(int id, BracketTeam bracketTeam)
        {
            if (id != bracketTeam.BracketId)
            {
                return BadRequest();
            }

            _context.Entry(bracketTeam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BracketTeamExists(id))
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

        // POST: api/BracketTeamControllerApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BracketTeam>> PostBracketTeam(BracketTeam bracketTeam)
        {
            _context.BracketTeam.Add(bracketTeam);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BracketTeamExists(bracketTeam.BracketId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBracketTeam", new { id = bracketTeam.BracketId }, bracketTeam);
        }

        // DELETE: api/BracketTeamControllerApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBracketTeam(int id)
        {
            var bracketTeam = await _context.BracketTeam.FindAsync(id);
            if (bracketTeam == null)
            {
                return NotFound();
            }

            _context.BracketTeam.Remove(bracketTeam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BracketTeamExists(int id)
        {
            return _context.BracketTeam.Any(e => e.BracketId == id);
        }
    }
}
