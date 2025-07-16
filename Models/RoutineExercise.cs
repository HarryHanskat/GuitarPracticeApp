namespace GuitarPracticeApp.Models
{
    public class RoutineExercise
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public int Order { get; set; }
        public int PracticeRoutineId { get; set; }
        public PracticeRoutine PracticeRoutine { get; set; } = null!;
    }
}
