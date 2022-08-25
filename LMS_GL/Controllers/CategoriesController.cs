using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS_GL.Models;
using Microsoft.AspNetCore.Authorization;

namespace LMS_GL.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly LMSContext _context;

        public CategoriesController(LMSContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _context.category != null ? 
                          View(await _context.category.ToListAsync()) :
                          Problem("Entity set 'LMSContext.category'  is null.");
        }
        public ActionResult GetCategories()
        {
            return View(_context.category.ToList());

        }

        public ActionResult AssignCourses(int id)
        {
            return View(_context.courses.Where(e => e.CategId == id).ToList());
        }
        [Authorize (Roles="admin")]
        public async Task<IActionResult> Adminindex()
        {
            return _context.category != null ?
                          View(await _context.category.ToListAsync()) :
                          Problem("Entity set 'LMSContext.category'  is null.");
        }
        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.category == null)
            {
                return NotFound();
            }

            var category = await _context.category
                .FirstOrDefaultAsync(m => m.CategId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.category == null)
            {
                return NotFound();
            }

            var category = await _context.category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategId,CategoryName")] Category category)
        {
            if (id != category.CategId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Category Edited Successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategId))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.category == null)
            {
                return NotFound();
            }

            var category = await _context.category
                .FirstOrDefaultAsync(m => m.CategId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.category == null)
            {
                return Problem("Entity set 'LMSContext.category'  is null.");
            }
            var category = await _context.category.FindAsync(id);
            if (category != null)
            {
                _context.category.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            TempData["success"] = "Category deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.category?.Any(e => e.CategId == id)).GetValueOrDefault();
        }
    }
}
