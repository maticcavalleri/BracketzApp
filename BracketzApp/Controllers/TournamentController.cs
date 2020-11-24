using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BracketzApp.Data;
using BracketzApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace BracketzApp.Controllers
{
    [Authorize]
    public class TournamentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public TournamentController(ApplicationDbContext context, UserManager<IdentityUser> userMgr)
        {
            _context = context;
            _userManager = userMgr;
        }

        // GET: Tournament
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tournament.Include(t => t.TournamentFormat).Include(t => t.User);
            var tournamentList = await applicationDbContext.ToListAsync();
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            IDictionary<int, bool> ownsTournament = new Dictionary<int, bool>();
            foreach (var tournament in tournamentList)
            {
                var isOwner = tournament.UserId == currentUser.Id ? true : false; 
                ownsTournament.Add(tournament.Id, isOwner);
            }

            ViewBag.ownsTournament = ownsTournament;

            return View(tournamentList);
        }

        // GET: Tournament/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournament
                .Include(t => t.TournamentFormat)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        // GET: Tournament/Create
        public IActionResult Create()
        {
            ViewData["TournamentFormatId"] = new SelectList(_context.TournamentFormat, "Id", "Name");
            return View();
        }

        // POST: Tournament/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NOfGames,Game,UserId,Date,TournamentFormatId")] Tournament tournament)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            tournament.UserId = currentUser.Id;

            if (ModelState.IsValid)
            {
                _context.Add(tournament);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TournamentFormatId"] = new SelectList(_context.TournamentFormat, "Id", "Name", tournament.TournamentFormatId);

            return View();
        }

        public async Task<IActionResult> Join(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var myTeam = await _context.Team.FirstOrDefaultAsync(x => x.OwnerId == currentUser.Id);
            if (myTeam != null)
            {
                myTeam.TournamentId = id;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // GET: Tournament/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournament.FindAsync(id);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (tournament == null || tournament.User != currentUser)
            {
                return NotFound();
            }

            ViewData["TournamentFormatId"] = new SelectList(_context.TournamentFormat, "Id", "Name", tournament.TournamentFormatId);
            return View(tournament);
        }

        // POST: Tournament/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NOfGames,Game,UserId,Date,TournamentFormatId")] Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tournament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentExists(tournament.Id))
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
            ViewData["TournamentFormatId"] = new SelectList(_context.TournamentFormat, "Id", "Name", tournament.TournamentFormatId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", tournament.UserId);
            return View(tournament);
        }

        // GET: Tournament/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournament
                .Include(t => t.TournamentFormat)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (tournament == null || tournament.User != currentUser)
            {
                return NotFound();
            }

            return View(tournament);
        }

        // POST: Tournament/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournament = await _context.Tournament.FindAsync(id);
            _context.Tournament.Remove(tournament);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TournamentExists(int id)
        {
            return _context.Tournament.Any(e => e.Id == id);
        }
    }
}
