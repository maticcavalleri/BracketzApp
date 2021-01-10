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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Generate")]
        public async Task<IActionResult> InitialBrackets([FromForm] BracketGenerateModel bracketGenerateModel)
        {
            var teams = _context.TournamentTeam
                .Where(m => m.TournamentId == bracketGenerateModel.TournamentId)
                .ToList();

            var brackets = new Dictionary<int, Bracket>();
            for (var i = teams.Count - 2; i >= 0; i--)
            {
                var parentId = (i == 0) ? -1 : (i - 1) / 2;
                var bracket = new Bracket()
                {
                    Index = i,
                    ParentId = parentId,
                    TournamentId = bracketGenerateModel.TournamentId,
                };
                brackets.Add(i, bracket);
                await _context.Bracket.AddAsync(bracket);
                await _context.SaveChangesAsync();
            }

            // generateInitialBrackets
            var j = 0;
            for (var i = teams.Count - 2; i >= (teams.Count - 2) / 2; i--)
            {
                brackets[i].Team1Id = teams[j++].TeamId;
                brackets[i].Team2Id = teams[j++].TeamId;
            }

            await _context.SaveChangesAsync();
            return Ok();
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
