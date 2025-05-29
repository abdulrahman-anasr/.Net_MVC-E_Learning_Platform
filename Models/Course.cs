using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcE_Learning.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [Display(Name = "Difficulty")]
        public string Difficulty { get; set; }

        [Display(Name = "Fields")]
        public string Fields { get; set; }

        [Display(Name = "Rating")]
        public float Rating { get; set; }
    
        









    }
}
