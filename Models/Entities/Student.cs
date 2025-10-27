using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentTimeTrackerApp.Data;
using StudentTimeTrackerApp.Models;
using StudentTimeTrackerApp.Models.Entities;

namespace StudentTimeTrackerApp.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string ANum { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Prefix? Prefix { get; set; }
        public Suffix? Suffix { get; set; }


        public string UserID { get; set; } = string.Empty;
        [ForeignKey("UserID")]
        public required ApplicationUser User { get; set; }


        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
