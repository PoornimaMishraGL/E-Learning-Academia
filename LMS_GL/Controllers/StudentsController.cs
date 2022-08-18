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
    public class StudentsController : Controller
    {
        private readonly LMSContext _context;
        public IHostingEnvironment _env;

        public StudentsController(LMSContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
              return _context.students != null ? 
                          View(await _context.students.ToListAsync()) :
                          Problem("Entity set 'LMSContext.students'  is null.");
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.students == null)
            {
                return NotFound();
            }

            var student = await _context.students
                .FirstOrDefaultAsync(m => m.StuId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            var nam = Path.Combine(_env.WebRootPath + "/Images", Path.GetFileName(student.StuImage.FileName));
            student.StuImage.CopyTo(new FileStream(nam, FileMode.Create));
            student.ImagePath = "Images/" + student.StuImage.FileName;
            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.students == null)
            {
                return NotFound();
            }

            var student = await _context.students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Student student)
        {
            if (id != student.StuId)
            {
                return NotFound();
            }

            
                try
                {
                var nam = Path.Combine(_env.WebRootPath + "/Images", Path.GetFileName(student.StuImage.FileName));
                student.StuImage.CopyTo(new FileStream(nam, FileMode.Create));
                student.ImagePath = "Images/" + student.StuImage.FileName;
                _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StuId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.students == null)
            {
                return NotFound();
            }

            var student = await _context.students
                .FirstOrDefaultAsync(m => m.StuId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.students == null)
            {
                return Problem("Entity set 'LMSContext.students'  is null.");
            }
            var student = await _context.students.FindAsync(id);
            if (student != null)
            {
                _context.students.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
          return (_context.students?.Any(e => e.StuId == id)).GetValueOrDefault();
        }
    }
}
