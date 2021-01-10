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
        //public DbSet<BracketTeam> BracketTeam { get; set; }
        public DbSet<TournamentFormat> TournamentFormat { get; set; }
        public DbSet<Tournament> Tournament { get; set; }
        public DbSet<TournamentTeam> TournamentTeam { get; set; }
        public DbSet<Participant> Participant { get; set; }
        public DbSet<ParticipantTeam> ParticipantTeam { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            /*modelBuilder.Entity<BracketTeam>().HasKey(t => new { t.BracketId, t.TeamId });

            modelBuilder.Entity<BracketTeam>()
                        .HasOne(t => t.Bracket)
                        .WithMany(t => t.BracketTeam)
                        .HasForeignKey(t => t.BracketId);

            modelBuilder.Entity<BracketTeam>()
                        .HasOne(t => t.Team)
                        .WithMany(t => t.BracketTeam)
                        .HasForeignKey(t => t.TeamId);*/

            modelBuilder.Entity<ParticipantTeam>().HasKey(t => new { t.ParticipantId, t.TeamId });

            modelBuilder.Entity<ParticipantTeam>()
                        .HasOne(t => t.Team)
                        .WithMany(t => t.ParticipantTeam)
                        .HasForeignKey(t => t.TeamId);

            modelBuilder.Entity<ParticipantTeam>()
                        .HasOne(t => t.Participant)
                        .WithMany(t => t.ParticipantTeam)
                        .HasForeignKey(t => t.ParticipantId);

            modelBuilder.Entity<TournamentTeam>().HasKey(t => new { t.TournamentId, t.TeamId });

            modelBuilder.Entity<TournamentTeam>()
                        .HasOne(t => t.Tournament)
                        .WithMany(t => t.TournamentTeam)
                        .HasForeignKey(t => t.TournamentId);

            modelBuilder.Entity<TournamentTeam>()
                        .HasOne(t => t.Team)
                        .WithMany(t => t.TournamentTeam)
                        .HasForeignKey(t => t.TeamId);
        }
    }
}