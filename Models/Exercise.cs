using System.ComponentModel.DataAnnotations;

namespace GuitarPracticeApp.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty; // e.g., "Scales", "Chords"
        public string? Description { get; set; }

        [Range(40, 300)]
        public int? Bpm { get; set; }

        public int PracticeSessionId { get; set; }

        public PracticeSession? PracticeSession { get; set; }
    }
}
