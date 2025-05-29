namespace MvcE_Learning.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public int Age { get; set; }

        public string? AreasOfInterest { get; set; }
        
        public string? EducationLevel { get; set; }

    }
}
