using System.ComponentModel.DataAnnotations;

namespace Tian_fullstack.Models
{
    public class Slide
    {
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int LessonId { get; set; }
        [Required]
        public int Order {  get; set; }
        [Required]
        public string Content { get; set; } // HTML content
        public List<string> Options { get; set; }
        [Range(1,4)]
        public int? CorrectOptionIndex { get; set; }
    }
}
