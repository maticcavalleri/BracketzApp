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
    public class BracketTeamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BracketTeamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BracketTeam
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BracketTeam.Include(b => b.Bracket).Include(b => b.Team);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BracketTeam/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracketTeam = await _context.BracketTeam
                .Include(b => b.Bracket)
                .Include(b => b.Team)
                .FirstOrDefaultAsync(m => m.BracketId == id);
            if (bracketTeam == null)
            {
                return NotFound();
            }

            return View(bracketTeam);
        }

        // GET: BracketTeam/Create
        public IActionResult Create()
        {
            ViewData["BracketId"] = new SelectList(_context.Bracket, "Id", "Id");
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name");
            return View();
        }

        // POST: BracketTeam/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BracketId,TeamId")] BracketTeam bracketTeam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bracketTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BracketId"] = new SelectList(_context.Bracket, "Id", "Id", bracketTeam.BracketId);
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name", bracketTeam.TeamId);
            return View(bracketTeam);
        }

        // GET: BracketTeam/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracketTeam = await _context.BracketTeam.FindAsync(id);
            if (bracketTeam == null)
            {
                return NotFound();
            }
            ViewData["BracketId"] = new SelectList(_context.Bracket, "Id", "Id", bracketTeam.BracketId);
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name", bracketTeam.TeamId);
            return View(bracketTeam);
        }

        // POST: BracketTeam/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BracketId,TeamId")] BracketTeam bracketTeam)
        {
            if (id != bracketTeam.BracketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bracketTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BracketTeamExists(bracketTeam.BracketId))
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
            ViewData["BracketId"] = new SelectList(_context.Bracket, "Id", "Id", bracketTeam.BracketId);
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name", bracketTeam.TeamId);
            return View(bracketTeam);
        }

        // GET: BracketTeam/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracketTeam = await _context.BracketTeam
                .Include(b => b.Bracket)
                .Include(b => b.Team)
                .FirstOrDefaultAsync(m => m.BracketId == id);
            if (bracketTeam == null)
            {
                return NotFound();
            }

            return View(bracketTeam);
        }

        // POST: BracketTeam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bracketTeam = await _context.BracketTeam.FindAsync(id);
            _context.BracketTeam.Remove(bracketTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BracketTeamExists(int id)
        {
            return _context.BracketTeam.Any(e => e.BracketId == id);
        }
    }
}
