using System.ComponentModel.DataAnnotations;

namespace LMS_GL.Models
{
    public class Category
    {
        [Key]
        public int CategId { get; set; }
        public string CategoryName { get; set; }

    }
}
