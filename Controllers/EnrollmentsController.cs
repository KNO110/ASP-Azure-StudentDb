using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;

namespace StudentPortal.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EnrollmentsController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var list = await _db.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var enrollment = await _db.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);
            if (enrollment == null) return NotFound();
            return View(enrollment);
        }

        public IActionResult Create()
        {
            ViewBag.Students = new SelectList(_db.Students, "StudentId", "FirstName");
            ViewBag.Courses = new SelectList(_db.Courses, "CourseId", "Title");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,CourseId,Grade")] Enrollment enrollment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Students = new SelectList(_db.Students, "StudentId", "FirstName", enrollment.StudentId);
                ViewBag.Courses = new SelectList(_db.Courses, "CourseId", "Title", enrollment.CourseId);
                return View(enrollment);
            }

            _db.Add(enrollment);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var enrollment = await _db.Enrollments.FindAsync(id);
            if (enrollment == null) return NotFound();

            ViewBag.Students = new SelectList(_db.Students, "StudentId", "FirstName", enrollment.StudentId);
            ViewBag.Courses = new SelectList(_db.Courses, "CourseId", "Title", enrollment.CourseId);
            return View(enrollment);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentId,StudentId,CourseId,Grade")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Students = new SelectList(_db.Students, "StudentId", "FirstName", enrollment.StudentId);
                ViewBag.Courses = new SelectList(_db.Courses, "CourseId", "Title", enrollment.CourseId);
                return View(enrollment);
            }

            try
            {
                _db.Update(enrollment);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Enrollments.AnyAsync(e => e.EnrollmentId == id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var enrollment = await _db.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);
            if (enrollment == null) return NotFound();
            return View(enrollment);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _db.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _db.Enrollments.Remove(enrollment);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
