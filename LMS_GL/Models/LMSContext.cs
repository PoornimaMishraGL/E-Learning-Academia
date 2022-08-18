using Microsoft.EntityFrameworkCore;

namespace LMS_GL.Models
{
    public class LMSContext:DbContext
    {
        
       public LMSContext(DbContextOptions<LMSContext> options) : base(options) {}
        public DbSet<Student> students { get; set; }
        public DbSet<Category> category { get; set; }
        public DbSet<Courses> courses { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<ContactUs> contactUs { get; set; }
       
    }
}

