using System.ComponentModel.DataAnnotations;
using StudentTimeTrackerApp.Entities;

namespace StudentTimeTrackerApp.Models.Entities
{
    /// <summary>
    /// The object used for Courses. Stores data to identify it 
    /// and to link it to both Student and Instructor objects.
    /// </summary>
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public int CourseNum { get; set; }
        public int SectionNum {  get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();


        /// <summary>
        /// Grabs the Course object parameters and turns them into strings.
        /// </summary>
        /// <returns>The Course object name.</returns>
        public string GetCourseName()
        {
            string CourseName = $"{this.CourseCode}-{this.CourseNum}-{this.SectionNum}";
            return CourseName;
        }
    }
}
