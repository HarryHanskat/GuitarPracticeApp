using System.ComponentModel.DataAnnotations;

namespace GuitarPracticeApp.Models
{
    public class PracticeGoal
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public GoalType Type { get; set; }

        [Required]
        public int TargetValue { get; set; } // Minutes for time goals, count for frequency goals

        [Required]
        public GoalPeriod Period { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property for tracking progress
        public List<PracticeSession> PracticeSessions { get; set; } = new();
    }

    public enum GoalType
    {
        TotalPracticeTime, // Total minutes practiced
        PracticeFrequency, // Number of practice sessions
        CategoryFocus, // Focus on specific category
        BpmImprovement, // BPM improvement for exercises
    }

    public enum GoalPeriod
    {
        Daily,
        Weekly,
        Monthly,
        Custom,
    }
}
