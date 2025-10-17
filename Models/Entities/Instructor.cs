using StudentTimeTrackerApp.Models;
using StudentTimeTrackerApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentTimeTrackerApp.Entities
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }
        public string ANum { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Prefix Prefix { get; set; }
        public Suffix Suffix { get; set; }
        public bool isAdmin { get; set; } = false;

        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
