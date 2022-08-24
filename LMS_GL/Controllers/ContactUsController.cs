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
    public class ContactUsController : Controller
    {
        private readonly LMSContext _context;

        public ContactUsController(LMSContext context)
        {
            _context = context;
        }

        // GET: ContactUs
        public async Task<IActionResult> Index()
        {
            /*var lMSContext = _context.contactUs.Include(c => c.student);
            return View(await lMSContext.ToListAsync());*/
            return View();
        }

        // GET: ContactUs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.contactUs == null)
            {
                return NotFound();
            }

            var contactUs = await _context.contactUs
                .Include(c => c.student)
                .FirstOrDefaultAsync(m => m.GmailId == id);
            if (contactUs == null)
            {
                return NotFound();
            }

            return View(contactUs);
        }

        // GET: ContactUs/Create
        public IActionResult Create()
        {
           /* ViewData["StuId"] = new SelectList(_context.students, "StuId", "FirstName");*/

            return View();
        }

        // POST: ContactUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StuId,Name,GmailId,PhoneNumber,Message")] ContactUs contactUs)
        {
           
                _context.Add(contactUs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: ContactUs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.contactUs == null)
            {
                return NotFound();
            }

            var contactUs = await _context.contactUs.FindAsync(id);
            if (contactUs == null)
            {
                return NotFound();
            }
            ViewData["StuId"] = new SelectList(_context.students, "StuId", "FirstName", contactUs.StuId);
            return View(contactUs);
        }

        // POST: ContactUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StuId,Name,GmailId,PhoneNumber,Message")] ContactUs contactUs)
        {
            if (id != contactUs.GmailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactUs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactUsExists(contactUs.GmailId))
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
            ViewData["StuId"] = new SelectList(_context.students, "StuId", "FirstName", contactUs.StuId);
            return View(contactUs);
        }

        // GET: ContactUs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.contactUs == null)
            {
                return NotFound();
            }

            var contactUs = await _context.contactUs
                .Include(c => c.student)
                .FirstOrDefaultAsync(m => m.GmailId == id);
            if (contactUs == null)
            {
                return NotFound();
            }

            return View(contactUs);
        }

        // POST: ContactUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.contactUs == null)
            {
                return Problem("Entity set 'LMSContext.contactUs'  is null.");
            }
            var contactUs = await _context.contactUs.FindAsync(id);
            if (contactUs != null)
            {
                _context.contactUs.Remove(contactUs);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactUsExists(string id)
        {
          return (_context.contactUs?.Any(e => e.Name == id)).GetValueOrDefault();
        }
    }
}
