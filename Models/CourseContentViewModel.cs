namespace MvcE_Learning.Models
{
    public class CourseContentViewModel
    {
        public Course Course { get; set; }
        public ICollection<Content> Contents { get; set; }
    }
}
