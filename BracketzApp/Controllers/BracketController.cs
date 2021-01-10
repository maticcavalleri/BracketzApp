using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BracketzApp.Data;
using BracketzApp.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace BracketzApp.Controllers
{
    public class BracketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BracketController> _logger;

        public BracketController(ApplicationDbContext context, ILogger<BracketController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Bracket
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bracket.Include(b => b.Parent).Include(b => b.Tournament);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bracket/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracket = await _context.Bracket
                .Include(b => b.Parent)
                .Include(b => b.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bracket == null)
            {
                return NotFound();
            }

            return View(bracket);
        }

        // GET: Bracket/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Bracket, "Id", "Id");
            ViewData["TournamentId"] = new SelectList(_context.Tournament, "Id", "Game");
            return View();
        }

        // POST: Bracket/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ScoreTeam1,ScoreTeam2,Index,IsFinished,TournamentId,ParentId")] Bracket bracket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bracket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Bracket, "Id", "Id", bracket.ParentId);
            ViewData["TournamentId"] = new SelectList(_context.Tournament, "Id", "Game", bracket.TournamentId);
            return View(bracket);
        }

        // GET: Bracket/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracket = await _context.Bracket.FindAsync(id);
            if (bracket == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Bracket, "Id", "Id", bracket.ParentId);
            ViewData["TournamentId"] = new SelectList(_context.Tournament, "Id", "Game", bracket.TournamentId);
            return View(bracket);
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

                int winner;
                if (bracket.ScoreTeam1 > bracket.ScoreTeam2)
                {
                    winner = 1;
                } else
                {
                    winner = 2;
                }

                return Ok(winner);
            }

            return NotFound();
        }

        // POST: Bracket/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ScoreTeam1,ScoreTeam2,Index,IsFinished,TournamentId,ParentId")] Bracket bracket)
        {
            if (id != bracket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bracket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BracketExists(bracket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Bracket, "Id", "Id", bracket.ParentId);
            ViewData["TournamentId"] = new SelectList(_context.Tournament, "Id", "Game", bracket.TournamentId);
            return View(bracket);
        }

        // GET: Bracket/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracket = await _context.Bracket
                .Include(b => b.Parent)
                .Include(b => b.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bracket == null)
            {
                return NotFound();
            }

            return View(bracket);
        }

        // POST: Bracket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bracket = await _context.Bracket.FindAsync(id);
            _context.Bracket.Remove(bracket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BracketExists(int id)
        {
            return _context.Bracket.Any(e => e.Id == id);
        }
    }
}
