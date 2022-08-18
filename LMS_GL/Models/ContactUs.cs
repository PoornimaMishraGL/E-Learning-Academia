using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_GL.Models
{
    public class ContactUs
    {
        
        [ForeignKey("student")]
        public int StuId { get; set; }
        public Student student { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]


        [Key]
        public string GmailId { get; set; }
        
        public string PhoneNumber { get; set; }
        [Required]
        public string Message { get; set; }

        
    }
}
