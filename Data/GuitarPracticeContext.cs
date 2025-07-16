using GuitarPracticeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GuitarPracticeApp.Data
{
    public class GuitarPracticeContext : DbContext
    {
        public GuitarPracticeContext(DbContextOptions<GuitarPracticeContext> options)
            : base(options) { }

        public DbSet<PracticeSession> PracticeSessions { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<PracticeRoutine> PracticeRoutines { get; set; }
        public DbSet<RoutineExercise> RoutineExercises { get; set; }
        public DbSet<PracticeGoal> PracticeGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder
                .Entity<Exercise>()
                .HasOne(e => e.PracticeSession)
                .WithMany(p => p.Exercises)
                .HasForeignKey(e => e.PracticeSessionId);

            modelBuilder
                .Entity<RoutineExercise>()
                .HasOne(re => re.PracticeRoutine)
                .WithMany(pr => pr.Exercises)
                .HasForeignKey(re => re.PracticeRoutineId);
        }
    }
}
