using StudentTimeTrackerApp.Data;
using StudentTimeTrackerApp.Models;
using StudentTimeTrackerApp.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTimeTrackerApp.Entities
{
    /// <summary>
    /// Represents an instructor. Holds data to identify that individual instructor.
    /// Instructor objects are linked to Course objects and, indirectly, to Students.
    /// </summary>
    public class Instructor
    {
        [Key]
        public int Id { get; set; }
        public string ANum { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Prefix? Prefix { get; set; }
        public Suffix? Suffix { get; set; }
        public bool isAdmin { get; set; } = false;

        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public required ApplicationUser User { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
