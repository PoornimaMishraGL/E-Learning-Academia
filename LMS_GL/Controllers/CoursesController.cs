using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS_GL.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace LMS_GL.Controllers
{
    public class CoursesController : Controller
    {
        private readonly LMSContext _context;
        public IHostingEnvironment _env;

        public CoursesController(LMSContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var lMSContext = _context.courses.Include(c => c.category);
            return View(await lMSContext.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.courses == null)
            {
                return NotFound();
            }

            var courses = await _context.courses
                .Include(c => c.category)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (courses == null)
            {
                return NotFound();
            }

            return View(courses);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["CategId"] = new SelectList(_context.category, "CategId", "CategId");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Courses courses)
        {
            
                var nam = Path.Combine(_env.WebRootPath + "/Images", Path.GetFileName(courses.CourseImage.FileName));
                courses.CourseImage.CopyTo(new FileStream(nam, FileMode.Create));
                courses.ImagePath = "Images/" + courses.CourseImage.FileName;
                _context.Add(courses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
            ViewData["CategId"] = new SelectList(_context.category, "CategId", "CategId", courses.CategId);
            return View(courses);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.courses == null)
            {
                return NotFound();
            }

            var courses = await _context.courses.FindAsync(id);
            if (courses == null)
            {
                return NotFound();
            }
            ViewData["CategId"] = new SelectList(_context.category, "CategId", "CategId", courses.CategId);
            return View(courses);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Courses courses)
        {
           
                var nam = Path.Combine(_env.WebRootPath + "/Images", Path.GetFileName(courses.CourseImage.FileName));
                courses.CourseImage.CopyTo(new FileStream(nam, FileMode.Create));
           
                courses.ImagePath = "Images/" + courses.CourseImage.FileName;
                _context.Update(courses);
                 await _context.SaveChangesAsync();
              
                return RedirectToAction(nameof(Index));
            
            //ViewData["CategId"] = new SelectList(_context.category, "CategId", "CategId", courses.CategId);
            //return View(courses);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.courses == null)
            {
                return NotFound();
            }

            var courses = await _context.courses
                .Include(c => c.category)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (courses == null)
            {
                return NotFound();
            }

            return View(courses);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.courses == null)
            {
                return Problem("Entity set 'LMSContext.courses'  is null.");
            }
            var courses = await _context.courses.FindAsync(id);
            if (courses != null)
            {
                _context.courses.Remove(courses);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoursesExists(int id)
        {
          return (_context.courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}
