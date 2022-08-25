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
    public class FeedbacksController : Controller
    {
        private readonly LMSContext _context;

        public FeedbacksController(LMSContext context)
        {
            _context = context;
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index()
        {
             /* return _context.feedbacks != null ? 
                          View(await _context.feedbacks.ToListAsync()) :
                          Problem("Entity set 'LMSContext.feedbacks'  is null.");*/
             return View();
        }

        

        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmailId,UserName,Suggestion")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                TempData["success"] = "Sent Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }

        



        private bool FeedbackExists(int id)
        {
          return (_context.feedbacks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
