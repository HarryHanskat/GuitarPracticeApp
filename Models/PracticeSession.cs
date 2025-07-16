namespace GuitarPracticeApp.Models
{
    public class PracticeSession
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int DurationMinutes { get; set; }
        public string? Notes { get; set; }
        public List<Exercise> Exercises { get; set; } = new();
    }
}
