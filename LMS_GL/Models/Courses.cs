using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_GL.Models
{
    public class Courses
    {
        [Key]
        public int CourseId { get; set; }
        [ForeignKey("category")]
        public int CategId { get; set; }
        public Category category { get; set; }
        public string Coursename { get; set; }
        [NotMapped]
        public IFormFile CourseImage { get; set; }
        public string ImagePath { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string duration { get; set; }
        public string MentorName { get; set; }
      
    }
}
