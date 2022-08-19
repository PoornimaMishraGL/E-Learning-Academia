using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_GL.Models
{
    public class Cart
    {
        [Key]
        public int id { get; set; }
        [NotMapped]
        public IFormFile CourseImage { get; set; }
        public string ImagePath { get; set; }
        
        [ForeignKey("student")]
        public int StuId { get; set; }
        public Student student { get; set; }

        [ForeignKey("courses")]
        public int CourseId { get; set; }
        public Courses courses { get; set; }
       
        public string Coursename { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string duration { get; set; }
        public string MentorName { get; set; }

    }
}
