using System.ComponentModel.DataAnnotations;

namespace Tian_fullstack.Models
{
    public class Slide
    {
        public int Id { get; set; }
        public int LessonNumber { get; set; }
        [Required]
        public int Order {  get; set; }
        [Required]
        public string Content { get; set; }
        public bool enableCodeEditor { get; set; }
        public string? ImagePath { get; set; }
        public List<string> Options { get; set; }
        [Range(1,4)]
        public int? CorrectOptionIndex { get; set; }
    }
}
