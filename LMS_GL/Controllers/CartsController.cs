using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS_GL.Models;
using System.Text;
using System.Security.Cryptography;
using LMS_GL.Data;

namespace LMS_GL.Controllers
{
    public class CartsController : Controller
    {
        private readonly LMSContext _context;
        public readonly ApplicationDbContext _DbContext;


        public CartsController(LMSContext context,ApplicationDbContext dbContext)
        {
            _context = context;
            _DbContext = dbContext;
        }
     
        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var lMSContext = _context.carts.Include(c => c.courses).Include(c => c.student);
            return View(await lMSContext.ToListAsync());
        }

        public async Task<IActionResult> createOrder(ApplicationUser _requestData, int? id)
        {
            //Courses cr = new Courses();

            string Amount;
         
            Courses courses = _context.courses.ToList().FirstOrDefault(e => e.CourseId == id );
            Amount = courses.Price;
            decimal amt = decimal.Parse(Amount);
            Student student = _context.students.ToList().FirstOrDefault(e => e.StuId ==id);
           

            Random randomObject = new Random();
            string transactionalId = randomObject.Next(100000, 100000).ToString();
            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_Qy5wk1RZJb8l5l", "DsTw4G8c0W0rhjrfD4MCBDjn");
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", amt * 100);
            // options.Add("recipt", transactionalId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "1");

            Razorpay.Api.Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();

           
            OrderModel orderModel = new OrderModel
            {
                orderId = orderResponse.Attributes["id"],
                razorpayKey = "rzp_test_Qy5wk1RZJb8l5l",
                amount = amt * 100,
                currency = "INR",
                name = student.FirstName + student.LastName,
                email =_requestData.Email,
                contactNumber = student.PhoneNumber,
               // address = _requestData.Address,
                description = courses.Description,

            };
            _DbContext.SaveChanges();
            return View("PaymentPage", orderModel);
        }

        public class OrderModel
        {
            public string orderId { get; set; }
            public string razorpayKey { get; set; }
            public decimal amount { get; set; }
            public string currency { get; set; }

            public string name { get; set; }
            public string email { get; set; }
            public string contactNumber { get; set; }
           // public string address { get; set; }
            public string description { get; set; }
        }

        public ViewResult AfterPayment()
        {
            var paymentStatus = Request.Form["paymentstatus"].ToString();
            if (paymentStatus == "Fail")
                return View("Fail");

            var orderId = Request.Form["orderid"].ToString();
            var paymentId = Request.Form["paymentid"].ToString();
            var signature = Request.Form["signature"].ToString();

            var validSignature = CompareSignatures(orderId, paymentId, signature);
            if (validSignature)
            {
                ViewBag.Message = "Congratulations!! Your payment was successful";
                return View("Success");
            }
            else
            {
                return View("Fail");
            }
        }

        private bool CompareSignatures(string orderId, string paymentId, string razorPaySignature)
        {
            var text = orderId + "|" + paymentId;
            var secret = "DsTw4G8c0W0rhjrfD4MCBDjn";
            var generatedSignature = CalculateSHA256(text, secret);
            if (generatedSignature == razorPaySignature)
                return true;
            else
                return false;
        }

        private string CalculateSHA256(string text, string secret)
        {
            string result = "";
            var enc = Encoding.Default;
            byte[]
            baText2BeHashed = enc.GetBytes(text),
            baSalt = enc.GetBytes(secret);
            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }




        public IActionResult Add_to_cart(int id)
        {

            Cart c = new Cart();
            Courses courses = _context.courses.ToList().FirstOrDefault(e=>e.CourseId == id);
           Student st=new Student();
           
            
            c.StuId = 1;
            c.CourseId = id;
            c.ImagePath = courses.ImagePath;
            c.Coursename = courses.Coursename;
            c.Price = courses.Price;
            c.Description = courses.Description;
            c.duration = courses.duration;
            c.MentorName = courses.MentorName;
            _context.carts.Add(c);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Create([Bind("id,ImagePath,StuId,CourseId,Coursename,Price,Description,duration,MentorName")] Cart cart)
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
        public async Task<IActionResult> Edit(int id, [Bind("id,ImagePath,StuId,CourseId,Coursename,Price,Description,duration,MentorName")] Cart cart)
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
//------------------------------------Buy Now----------------------------------
        public async Task<IActionResult> BuyNow(int? id)
        {
            var cart = await _context.carts
                .Include(c => c.courses)
                .Include(c => c.student)
                .FirstOrDefaultAsync(m => m.id == id);
          
            return View(cart);
        }
    }
}
