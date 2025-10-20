using System.ComponentModel.DataAnnotations;
using StudentTimeTrackerApp.Entities;

namespace StudentTimeTrackerApp.Models.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public int CourseNum { get; set; }
        public int SectionNum {  get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();


        public string GetCourseName(string CourseCode, int CourseNum, int SectionNum)
        {
            string CourseName = string.Empty;
            CourseName = $"{CourseCode}-{CourseNum}-{SectionNum}";
            return CourseName;
        }
    }
}
