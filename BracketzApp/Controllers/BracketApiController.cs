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
    public class BracketApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BracketApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BracketApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bracket>>> GetBracket()
        {
            return await _context.Bracket.ToListAsync();
        }

        // GET: api/BracketApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bracket>> GetBracket(int id)
        {
            var bracket = await _context.Bracket.FindAsync(id);

            if (bracket == null)
            {
                return NotFound();
            }

            return bracket;
        }

        // PUT: api/BracketApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBracket(int id, Bracket bracket)
        {
            if (id != bracket.Id)
            {
                return BadRequest();
            }

            _context.Entry(bracket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BracketExists(id))
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

        // POST: api/BracketApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bracket>> PostBracket(Bracket bracket)
        {
            _context.Bracket.Add(bracket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBracket", new { id = bracket.Id }, bracket);
        }

        // DELETE: api/BracketApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBracket(int id)
        {
            var bracket = await _context.Bracket.FindAsync(id);
            if (bracket == null)
            {
                return NotFound();
            }

            _context.Bracket.Remove(bracket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BracketExists(int id)
        {
            return _context.Bracket.Any(e => e.Id == id);
        }
    }
}
