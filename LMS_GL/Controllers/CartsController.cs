using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS_GL.Models;

namespace LMS_GL.Controllers
{
    public class CartsController : Controller
    {
        private readonly LMSContext _context;

        public CartsController(LMSContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var lMSContext = _context.carts.Include(c => c.courses).Include(c => c.student);
            return View(await lMSContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)

        {
            if (id == null || _context.carts == null)
            {
                return NotFound();
            }

            var cart = await _context.carts
                .Include(c => c.courses)
                .Include(c => c.student)
                .FirstOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.courses, "CourseId", "CourseId");
            ViewData["StuId"] = new SelectList(_context.students, "StuId", "FirstName");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,StuId,CourseId,Coursename,Price,Description,duration,MentorName")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.courses, "CourseId", "CourseId", cart.CourseId);
            ViewData["StuId"] = new SelectList(_context.students, "StuId", "FirstName", cart.StuId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.carts == null)
            {
                return NotFound();
            }

            var cart = await _context.carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.courses, "CourseId", "CourseId", cart.CourseId);
            ViewData["StuId"] = new SelectList(_context.students, "StuId", "FirstName", cart.StuId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,StuId,CourseId,Coursename,Price,Description,duration,MentorName")] Cart cart)
        {
            if (id != cart.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.id))
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
            ViewData["CourseId"] = new SelectList(_context.courses, "CourseId", "CourseId", cart.CourseId);
            ViewData["StuId"] = new SelectList(_context.students, "StuId", "FirstName", cart.StuId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.carts == null)
            {
                return NotFound();
            }

            var cart = await _context.carts
                .Include(c => c.courses)
                .Include(c => c.student)
                .FirstOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.carts == null)
            {
                return Problem("Entity set 'LMSContext.carts'  is null.");
            }
            var cart = await _context.carts.FindAsync(id);
            if (cart != null)
            {
                _context.carts.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
          return (_context.carts?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
