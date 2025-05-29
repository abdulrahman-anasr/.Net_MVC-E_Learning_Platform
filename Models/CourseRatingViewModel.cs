namespace MvcE_Learning.Models
{
    public class CourseRatingViewModel
    {
        public Course course { get; set; }
        public bool rated { get; set; }

        public int ratingId { get; set; }
        public float rating { get; set; }
    }
}
