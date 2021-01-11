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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("UpdateScore")]
        public async Task<IActionResult> UpdateScore([FromForm] BracketScoreUpdateModel bracketScoreUpdateModel)
        {
            var bracket = await _context.Bracket.FindAsync(bracketScoreUpdateModel.BracketId);
            if (bracket != null)
            {
                bracket.ScoreTeam1 = bracketScoreUpdateModel.ScoreTeam1;
                bracket.ScoreTeam2 = bracketScoreUpdateModel.ScoreTeam2;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("MarkFinished")]
        public async Task<IActionResult> MarkFinished([FromForm] MarkFinishedModel markFinishedModel)
        {
            var bracket = await _context.Bracket.FindAsync(markFinishedModel.BracketId);
            if (bracket != null)
            {
                bracket.IsFinished = true;
                await _context.SaveChangesAsync();

                // get winner id
                int winnerId;
                if (bracket.ScoreTeam1 > bracket.ScoreTeam2)
                {
                    winnerId = (int)bracket.Team1Id;
                }
                else
                {
                    winnerId = (int)bracket.Team2Id;
                }

                // get parent bracket
                var parentBracket = await _context.Bracket.FirstAsync(x =>
                    x.Index == markFinishedModel.ParentBracketIndex &&
                    x.TournamentId == markFinishedModel.TournamentId);

                // if team is already in parent bracket return BadRequest
                if (parentBracket.Team1Id == winnerId || parentBracket.Team2Id == winnerId)
                    return BadRequest("winner team is already in parent bracket");

                // insert winner id in parent bracket
                if (parentBracket.Team1Id == null)
                {
                    parentBracket.Team1Id = winnerId;
                }
                else if (parentBracket.Team2Id == null)
                {
                    parentBracket.Team2Id = winnerId;
                }

                await _context.SaveChangesAsync();
                return Ok(winnerId);
            }

            return NotFound();
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
