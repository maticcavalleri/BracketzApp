using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using BracketzApp.Models;

namespace BracketzApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Team> Team { get; set; }
        public DbSet<Bracket> Bracket { get; set; }
        public DbSet<BracketTeam> BracketTeam { get; set; }
        public DbSet<TournamentFormat> TournamentFormat { get; set; }
        public DbSet<Tournament> Tournament { get; set; }
        public DbSet<Participant> Participant { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<BracketTeam>().HasKey(t => new { t.BracketId, t.TeamId });

            modelBuilder.Entity<BracketTeam>()
                        .HasOne(t => t.Bracket)
                        .WithMany(t => t.BracketTeam)
                        .HasForeignKey(t => t.BracketId);

            modelBuilder.Entity<BracketTeam>()
                        .HasOne(t => t.Team)
                        .WithMany(t => t.BracketTeam)
                        .HasForeignKey(t => t.TeamId);
        }
    }
}