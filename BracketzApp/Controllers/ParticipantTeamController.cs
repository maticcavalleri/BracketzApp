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
    public class ParticipantTeamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParticipantTeamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ParticipantTeam
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ParticipantTeam.Include(p => p.Participant).Include(p => p.Team);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ParticipantTeam/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantTeam = await _context.ParticipantTeam
                .Include(p => p.Participant)
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participantTeam == null)
            {
                return NotFound();
            }

            return View(participantTeam);
        }

        // GET: ParticipantTeam/Create
        public IActionResult Create()
        {
            ViewData["ParticipantId"] = new SelectList(_context.Participant, "Id", "Name");
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name");
            return View();
        }

        // POST: ParticipantTeam/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParticipantId,TeamId")] ParticipantTeam participantTeam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participantTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParticipantId"] = new SelectList(_context.Participant, "Id", "Name", participantTeam.ParticipantId);
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name", participantTeam.TeamId);
            return View(participantTeam);
        }

        // GET: ParticipantTeam/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantTeam = await _context.ParticipantTeam.FindAsync(id);
            if (participantTeam == null)
            {
                return NotFound();
            }
            ViewData["ParticipantId"] = new SelectList(_context.Participant, "Id", "Name", participantTeam.ParticipantId);
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name", participantTeam.TeamId);
            return View(participantTeam);
        }

        // POST: ParticipantTeam/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParticipantId,TeamId")] ParticipantTeam participantTeam)
        {
            if (id != participantTeam.ParticipantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participantTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantTeamExists(participantTeam.ParticipantId))
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
            ViewData["ParticipantId"] = new SelectList(_context.Participant, "Id", "Name", participantTeam.ParticipantId);
            ViewData["TeamId"] = new SelectList(_context.Team, "TeamId", "Name", participantTeam.TeamId);
            return View(participantTeam);
        }

        // GET: ParticipantTeam/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantTeam = await _context.ParticipantTeam
                .Include(p => p.Participant)
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participantTeam == null)
            {
                return NotFound();
            }

            return View(participantTeam);
        }

        // POST: ParticipantTeam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var participantTeam = await _context.ParticipantTeam.FindAsync(id);
            _context.ParticipantTeam.Remove(participantTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantTeamExists(int id)
        {
            return _context.ParticipantTeam.Any(e => e.ParticipantId == id);
        }
    }
}
