using GuitarPracticeApp.Data;
using GuitarPracticeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuitarPracticeApp.Controllers
{
    public class PracticeGoalsController : Controller
    {
        private readonly GuitarPracticeContext _context;

        public PracticeGoalsController(GuitarPracticeContext context)
        {
            _context = context;
        }
        // GET: PracticeGoals
        public async Task<IActionResult> Index()
        {
            var goals = await _context.PracticeGoals
                .OrderByDescending(g => g.IsActive)
                .ThenBy(g => g.EndDate)
                .ToListAsync();

            // Create a dictionary to store progress for each goal
            var goalProgress = new Dictionary<int, (int progress, int percentage)>();

            foreach (var goal in goals)
            {
                var sessions = await _context.PracticeSessions
                    .Include(s => s.Exercises)
                    .Where(s => s.Date >= goal.StartDate && s.Date <= goal.EndDate)
                    .ToListAsync();

                int currentProgress = 0;
                switch (goal.Type)
                {
                    case GoalType.TotalPracticeTime:
                        currentProgress = sessions.Sum(s => s.DurationMinutes);
                        break;
                    case GoalType.PracticeFrequency:
                        currentProgress = sessions.Count;
                        break;
                }

                var percentage = goal.TargetValue > 0 
                    ? Math.Min(100, (currentProgress * 100) / goal.TargetValue)
                    : 0;

                goalProgress[goal.Id] = (currentProgress, percentage);
            }

            ViewBag.GoalProgress = goalProgress;
            return View(goals);
        }

        // GET: PracticeGoals/Create
        public IActionResult Create()
        {
            var model = new PracticeGoal
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(30), // Default to 30 days
            };
            return View(model);
        }

        // POST: PracticeGoals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Description,Type,TargetValue,Period,StartDate,EndDate")]
                PracticeGoal practiceGoal
        )
        {
            if (ModelState.IsValid)
            {
                // Set dates based on period if not custom
                if (practiceGoal.Period != GoalPeriod.Custom)
                {
                    practiceGoal.StartDate = DateTime.Today;
                    practiceGoal.EndDate = practiceGoal.Period switch
                    {
                        GoalPeriod.Daily => DateTime.Today.AddDays(1),
                        GoalPeriod.Weekly => DateTime.Today.AddDays(7),
                        GoalPeriod.Monthly => DateTime.Today.AddMonths(1),
                        _ => practiceGoal.EndDate,
                    };
                }

                _context.Add(practiceGoal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(practiceGoal);
        }

        // GET: PracticeGoals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practiceGoal = await _context.PracticeGoals
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (practiceGoal == null)
            {
                return NotFound();
            }

            // Calculate progress for this specific goal
            var sessions = await _context.PracticeSessions
                .Include(s => s.Exercises)
                .Where(s => s.Date >= practiceGoal.StartDate && s.Date <= practiceGoal.EndDate)
                .ToListAsync();

            int currentProgress = 0;
            switch (practiceGoal.Type)
            {
                case GoalType.TotalPracticeTime:
                    currentProgress = sessions.Sum(s => s.DurationMinutes);
                    break;
                case GoalType.PracticeFrequency:
                    currentProgress = sessions.Count;
                    break;
            }

            ViewBag.CurrentProgress = currentProgress;
            ViewBag.ProgressPercentage = practiceGoal.TargetValue > 0 
                ? Math.Min(100, (currentProgress * 100) / practiceGoal.TargetValue)
                : 0;

            return View(practiceGoal);
        }

        // POST: PracticeGoals/ToggleActive/5
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var goal = await _context.PracticeGoals.FindAsync(id);
            if (goal != null)
            {
                goal.IsActive = !goal.IsActive;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task CalculateGoalProgress(PracticeGoal goal)
        {
            var sessions = await _context
                .PracticeSessions.Include(s => s.Exercises)
                .Where(s => s.Date >= goal.StartDate && s.Date <= goal.EndDate)
                .ToListAsync();

            switch (goal.Type)
            {
                case GoalType.TotalPracticeTime:
                    ViewBag.CurrentProgress = sessions.Sum(s => s.DurationMinutes);
                    break;
                case GoalType.PracticeFrequency:
                    ViewBag.CurrentProgress = sessions.Count;
                    break;
                // Add more goal types as needed
            }

            ViewBag.ProgressPercentage =
                goal.TargetValue > 0
                    ? Math.Min(100, (ViewBag.CurrentProgress * 100) / goal.TargetValue)
                    : 0;
        }
    }
}
