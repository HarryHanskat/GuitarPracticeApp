namespace GuitarPracticeApp.Models
{
    public class PracticeRoutine
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<RoutineExercise> Exercises { get; set; } = new();
    }
}
