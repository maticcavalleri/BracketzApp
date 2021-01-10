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
            var myTeam = await _context.Team.FirstOrDefaultAsync(x => x.OwnerId == currentUser.Id);

            ViewBag.ownsTournament = ownsTournament;

            var tournaments = _context.Tournament;
            var applicableTeams = new Dictionary<int, SelectList>();
            foreach (Tournament tournament in tournaments)
            {
                var userTeamsInTournament = _context.TournamentTeam.Where(m => m.Team.OwnerId == currentUser.Id && m.TournamentId == tournament.Id).ToList();
                var userTeamIdsInTournament = new List<int>();
                foreach (TournamentTeam tTeam in userTeamsInTournament)
                {
                    userTeamIdsInTournament.Add(tTeam.TeamId);
                }

                var userTeams = _context.Team.Where(m => m.OwnerId == currentUser.Id).ToList();
                var filteredUserTeamIds = new List<int>();
                foreach (Team team in userTeams)
                {
                    if (!userTeamIdsInTournament.Contains(team.TeamId))
                    {
                        filteredUserTeamIds.Add(team.TeamId);
                    }
                }
                var selectTeamList = _context.Team.Where(m => filteredUserTeamIds.Contains(m.TeamId)).ToList();

                applicableTeams.Add(tournament.Id, new SelectList(selectTeamList, "TeamId", "Name", myTeam.TeamId));
                //ViewData["TeamId"] = new SelectList(selectTeamList, "TeamId", "Name", myTeam.TeamId);
            }

            ViewData["ApplicableTeams"] = applicableTeams;

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
            
            return View(tournament);
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Join([FromForm] JoinTournamentModel joinTournamentModel)
        {
            var tournamentTeamEntry = new TournamentTeam
            {
                TournamentId = joinTournamentModel.TournamentId,
                TeamId = joinTournamentModel.TeamId
            };
            await _context.TournamentTeam.AddAsync(tournamentTeamEntry);
            _context.SaveChanges();
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
                usefulBrackets.Add(bracket.Index, new UsefulBracket()
                {
                    Id = bracket.Id,
                    Index = bracket.Index,
                    IsFinished = bracket.IsFinished,
                    ParentIndex = bracket.ParentId,
                    ScoreTeam1 = bracket.ScoreTeam1,
                    ScoreTeam2 = bracket.ScoreTeam2,
                    TournamentId = bracket.TournamentId,
                    Team1 = bracket.Team1,
                    Team2 = bracket.Team2,
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
