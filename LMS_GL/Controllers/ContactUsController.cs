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
            TempData["success"] = "Message Sent Successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool ContactUsExists(string id)
        {
          return (_context.contactUs?.Any(e => e.Name == id)).GetValueOrDefault();
        }
    }
}
