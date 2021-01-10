using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BracketzApp.CustomClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BracketzApp.Data;
using BracketzApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BracketzApp.Controllers
{
    public class TournamentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TournamentController> _logger;
        private UserManager<IdentityUser> _userManager;

        public TournamentController(
            ApplicationDbContext context, 
            UserManager<IdentityUser> userManager,
            ILogger<TournamentController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
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

            var teams = _context.TournamentTeam
                .Where(m => m.TournamentId == id)
                .ToList();
            
            ViewBag.teams = teams;
            ViewBag.tournament = tournament;

            // when you click details tournament starts and initial brackets are made
            await InitialBrackets(id);
            
            return View(tournament);
        }

        public async Task InitialBrackets(int? id)
        {
            var teams = _context.TournamentTeam
                .Where(m => m.TournamentId == id)
                .ToList();
            
            var brackets = new Dictionary<int, Bracket>();
            for (var i = teams.Count - 2; i >= 0; i--)
            {
                var parentId = (i == 0) ? -1 : (i - 1) / 2;
                var bracket = new Bracket()
                {
                    Index = i,
                    ParentId = parentId,
                    TournamentId = id,
                };
                brackets.Add(i, bracket);
                await _context.Bracket.AddAsync(bracket);
                await _context.SaveChangesAsync();
            }
            
            // generateInitialBrackets
            var j = 0;
            for (var i = teams.Count - 2; i >= (teams.Count - 2) / 2; i--)
            {
                var BrTeam1 = new BracketTeam
                {
                    BracketId = brackets[i].Id,
                    TeamId = teams[j++].TeamId
                };
                var BrTeam2 = new BracketTeam
                {
                    BracketId = brackets[i].Id,
                    TeamId = teams[j++].TeamId
                };
                await _context.BracketTeam.AddAsync(BrTeam1);
                await _context.SaveChangesAsync();
                await _context.BracketTeam.AddAsync(BrTeam2);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
        }

        // GET: Tournament/Create
        public IActionResult Create()
        {
            ViewData["TournamentFormatId"] = new SelectList(_context.TournamentFormat, "Id", "Name");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
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
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", tournament.UserId);
            return View(tournament);
        }

        public async Task<IActionResult> Join(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var myTeam = await _context.Team.FirstOrDefaultAsync(x => x.OwnerId == currentUser.Id);
            if (myTeam != null)
            {
                var tournamentTeamEntry = new TournamentTeam();
                tournamentTeamEntry.TournamentId = (int)id;
                tournamentTeamEntry.TeamId = myTeam.TeamId;
                var teamEntry = await _context.TournamentTeam.AddAsync(tournamentTeamEntry);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Leave(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var myTeam = await _context.Team.FirstOrDefaultAsync(x => x.OwnerId == currentUser.Id);
            if (myTeam != null)
            {
                var tournamentTeamEntry = new TournamentTeam();
                tournamentTeamEntry.TournamentId = (int)id;
                tournamentTeamEntry.TeamId = myTeam.TeamId;
                var teamEntry = _context.TournamentTeam.Remove(tournamentTeamEntry);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Inspect(int? id)
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

            var tournamentTeams = _context.TournamentTeam
                .Where(m => m.TournamentId == id)
                .ToList();

            List<int> teamIds = new List<int>();
            foreach (TournamentTeam tournamentTeam in tournamentTeams) {
                _logger.LogInformation(tournamentTeam.TeamId.ToString());
                teamIds.Add(tournamentTeam.TeamId);
            }

            var teams = _context.Team.Where(r => teamIds.Contains(r.TeamId)).ToList();

            var brackets = _context.Bracket.Where(x => x.TournamentId.Equals(id));
            var usefulBrackets = new Dictionary<int, UsefulBracket>();
            foreach (var bracket in brackets)
            {
                var timIds = _context.BracketTeam.Where(x => x.BracketId == bracket.Id).ToList();
                usefulBrackets.Add(bracket.Index, new UsefulBracket()
                {
                    Id = bracket.Id,
                    Index = bracket.Index,
                    IsFinished = bracket.IsFinished,
                    ParentIndex = bracket.ParentId,
                    ScoreTeam1 = bracket.ScoreTeam1,
                    ScoreTeam2 = bracket.ScoreTeam2,
                    TournamentId = bracket.TournamentId,
                    Teams = timIds.Select(a => _context.Team.First(x => x.TeamId == a.TeamId)).ToArray(),
                });
            }

            ViewBag.brackets = usefulBrackets;
            ViewBag.teams = teams;
            return View(tournament);
        }

        // GET: Tournament/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournament.FindAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }
            ViewData["TournamentFormatId"] = new SelectList(_context.TournamentFormat, "Id", "Name", tournament.TournamentFormatId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", tournament.UserId);
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
