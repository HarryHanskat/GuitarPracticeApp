using GuitarPracticeApp.Data;
using GuitarPracticeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuitarPracticeApp.Controllers
{
    public class PracticeSessionsController : Controller
    {
        private readonly GuitarPracticeContext _context;

        public PracticeSessionsController(GuitarPracticeContext context)
        {
            _context = context;
        }

        // GET: PracticeSessions
        public async Task<IActionResult> Index()
        {
            var sessions = await _context
                .PracticeSessions.Include(p => p.Exercises)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
            return View(sessions);
        }

        // GET: PracticeSessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PracticeSessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Date,DurationMinutes,Notes")] PracticeSession practiceSession
        )
        {
            if (ModelState.IsValid)
            {
                _context.Add(practiceSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(practiceSession);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practiceSession = await _context
                .PracticeSessions.Include(p => p.Exercises)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (practiceSession == null)
            {
                return NotFound();
            }

            return View(practiceSession);
        }

        public async Task<IActionResult> AddExercise(int sessionId)
        {
            // Verify the session exists
            var session = await _context.PracticeSessions.FindAsync(sessionId);
            if (session == null)
            {
                return NotFound();
            }

            ViewBag.SessionId = sessionId;
            ViewBag.SessionDate = session.Date.ToString("yyyy-MM-dd");
            return View();
        }

        // POST: PracticeSessions/AddExercise
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExercise(
            [Bind("Name,Category,Description,Bpm,PracticeSessionId")] Exercise exercise
        )
        {
            // Debug: Check what's being received
            Console.WriteLine(
                $"Received exercise: Name={exercise.Name}, Category={exercise.Category}, SessionId={exercise.PracticeSessionId}"
            );

            // Validate that the session exists
            var sessionExists = await _context.PracticeSessions.AnyAsync(s =>
                s.Id == exercise.PracticeSessionId
            );
            if (!sessionExists)
            {
                ModelState.AddModelError("", "Invalid practice session.");
                ViewBag.SessionId = exercise.PracticeSessionId;
                return View(exercise);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(exercise);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Exercise saved successfully");
                    return RedirectToAction(
                        nameof(Details),
                        new { id = exercise.PracticeSessionId }
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving exercise: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the exercise.");
                }
            }
            else
            {
                // Debug: Show validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
            }

            ViewBag.SessionId = exercise.PracticeSessionId;
            return View(exercise);
        }
    }
}
