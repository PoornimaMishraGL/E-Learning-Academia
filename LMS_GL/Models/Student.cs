using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_GL.Models
{
    public class Student
    {
        [Key]
        public int StuId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [NotMapped]
        public IFormFile StuImage { get; set; }
        public string ImagePath { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
