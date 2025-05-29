namespace MvcE_Learning.Models
{
    public class Rating
    {
        public int RatingId { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public float rating { get; set; }
    }
}
