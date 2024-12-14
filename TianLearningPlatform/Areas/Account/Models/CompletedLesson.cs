namespace Tian_fullstack.Areas.Account.Models
{
    public class CompletedLesson
    {
        public int? Id { get; set; }
        public int LessonNumber { get; set; } = 0;
        public DateTime UpdatedAt { get; set; }
        public string UserId { get; set; }

    }
}
