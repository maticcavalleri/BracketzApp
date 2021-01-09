using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BracketzApp.Data;

namespace BracketzApp.Controllers
{
    public class TournamentTeamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TournamentTeamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TournamentTeam
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TournamentTeam.Include(t => t.Team).Include(t => t.Tournament);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TournamentTeam/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournamentTeam = await _context.TournamentTeam
                .Include(t => t.Team)
                .Include(t => t.Tournament)
                .FirstOrDefaultAsync(m => m.TournamentId == id);
            if (tournamentTeam == null)
            {
                return NotFound();
            }

            return View(tournamentTeam);
        }

        // GET: TournamentTeam/Create
        public IActionResult Create()
        {
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name");
            ViewData["TournamentId"] = new SelectList(_context.Tournament, "Id", "Game");
            return View();
        }

        // POST: TournamentTeam/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TournamentId,TeamId")] TournamentTeam tournamentTeam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tournamentTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name", tournamentTeam.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournament, "Id", "Game", tournamentTeam.TournamentId);
            return View(tournamentTeam);
        }

        // GET: TournamentTeam/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournamentTeam = await _context.TournamentTeam.FindAsync(id);
            if (tournamentTeam == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name", tournamentTeam.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournament, "Id", "Game", tournamentTeam.TournamentId);
            return View(tournamentTeam);
        }

        // POST: TournamentTeam/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TournamentId,TeamId")] TournamentTeam tournamentTeam)
        {
            if (id != tournamentTeam.TournamentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tournamentTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentTeamExists(tournamentTeam.TournamentId))
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
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name", tournamentTeam.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournament, "Id", "Game", tournamentTeam.TournamentId);
            return View(tournamentTeam);
        }

        // GET: TournamentTeam/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournamentTeam = await _context.TournamentTeam
                .Include(t => t.Team)
                .Include(t => t.Tournament)
                .FirstOrDefaultAsync(m => m.TournamentId == id);
            if (tournamentTeam == null)
            {
                return NotFound();
            }

            return View(tournamentTeam);
        }

        // POST: TournamentTeam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournamentTeam = await _context.TournamentTeam.FindAsync(id);
            _context.TournamentTeam.Remove(tournamentTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TournamentTeamExists(int id)
        {
            return _context.TournamentTeam.Any(e => e.TournamentId == id);
        }
    }
}
