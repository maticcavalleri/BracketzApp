using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BracketzApp.Data;
using BracketzApp.Models;

namespace BracketzApp.Controllers
{
    public class TournamentFormatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TournamentFormatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TournamentFormat
        public async Task<IActionResult> Index()
        {
            return View(await _context.TournamentFormat.ToListAsync());
        }

        // GET: TournamentFormat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournamentFormat = await _context.TournamentFormat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournamentFormat == null)
            {
                return NotFound();
            }

            return View(tournamentFormat);
        }

        // GET: TournamentFormat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TournamentFormat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TournamentFormat tournamentFormat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tournamentFormat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tournamentFormat);
        }

        // GET: TournamentFormat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournamentFormat = await _context.TournamentFormat.FindAsync(id);
            if (tournamentFormat == null)
            {
                return NotFound();
            }
            return View(tournamentFormat);
        }

        // POST: TournamentFormat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TournamentFormat tournamentFormat)
        {
            if (id != tournamentFormat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tournamentFormat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentFormatExists(tournamentFormat.Id))
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
            return View(tournamentFormat);
        }

        // GET: TournamentFormat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournamentFormat = await _context.TournamentFormat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournamentFormat == null)
            {
                return NotFound();
            }

            return View(tournamentFormat);
        }

        // POST: TournamentFormat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournamentFormat = await _context.TournamentFormat.FindAsync(id);
            _context.TournamentFormat.Remove(tournamentFormat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TournamentFormatExists(int id)
        {
            return _context.TournamentFormat.Any(e => e.Id == id);
        }
    }
}
