using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace Tian_fullstack.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Order { get; set; }
        [Required]
        public string Title { get; set; }
        public List<Slide> Slides { get; set; }
    }
}