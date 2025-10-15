using System.ComponentModel.DataAnnotations;

namespace StudentTimeTrackerApp.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string ANum { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
