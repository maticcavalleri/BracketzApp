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
    public class TournamentFormatApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TournamentFormatApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TournamentFormatControllerApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentFormat>>> GetTournamentFormat()
        {
            return await _context.TournamentFormat.ToListAsync();
        }

        // GET: api/TournamentFormatControllerApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentFormat>> GetTournamentFormat(int id)
        {
            var tournamentFormat = await _context.TournamentFormat.FindAsync(id);

            if (tournamentFormat == null)
            {
                return NotFound();
            }

            return tournamentFormat;
        }

        // PUT: api/TournamentFormatControllerApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentFormat(int id, TournamentFormat tournamentFormat)
        {
            if (id != tournamentFormat.Id)
            {
                return BadRequest();
            }

            _context.Entry(tournamentFormat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TournamentFormatExists(id))
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

        // POST: api/TournamentFormatControllerApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentFormat>> PostTournamentFormat(TournamentFormat tournamentFormat)
        {
            _context.TournamentFormat.Add(tournamentFormat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTournamentFormat", new { id = tournamentFormat.Id }, tournamentFormat);
        }

        // DELETE: api/TournamentFormatControllerApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentFormat(int id)
        {
            var tournamentFormat = await _context.TournamentFormat.FindAsync(id);
            if (tournamentFormat == null)
            {
                return NotFound();
            }

            _context.TournamentFormat.Remove(tournamentFormat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TournamentFormatExists(int id)
        {
            return _context.TournamentFormat.Any(e => e.Id == id);
        }
    }
}
