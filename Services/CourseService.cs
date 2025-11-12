using System;
using System.Linq;
using StudentTimeTrackerApp.Models.Entities;
using StudentTimeTrackerApp.Data;

namespace StudentTimeTrackerApp.Services
{
    public class CourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CreateCourse(string userId, string courseCode, int courseNum, int sectionNum)
        {
            string errorMessage = string.Empty;
            var instructor = _context.Instructors
                .FirstOrDefault(i => i.UserId == userId);

            if (instructor == null) {
                errorMessage = "Instructor was not found. Please try again.";
                // throw new InvalidOperationException("Instructor not found.");
                return errorMessage;
            }

            var course = new Course
            {
                
                CourseCode = courseCode,
                CourseNum = courseNum,
                SectionNum = sectionNum,
            };

            course.Instructors.Add(instructor);

            _context.Courses.Add(course);
            _context.SaveChanges();
            return errorMessage;
        }


        public string FindCourse(string userId, string courseCode, int courseNum, int sectionNum)
        {
            string errorMessage = string.Empty;
            var student = _context.Students
                .FirstOrDefault(s => s.UserID == userId);

            if (student == null) {
                errorMessage = "No student found. Please try again.";
                // throw new InvalidOperationException("No student found.");
                return errorMessage;
            }


            var match = _context.Courses
                .FirstOrDefault(c =>
                c.CourseCode == courseCode &&
                c.CourseNum == courseNum &&
                c.SectionNum == sectionNum);

            if (match == null)
            {
                errorMessage = "No course was found. Please try again.";
                // throw new InvalidOperationException("No Course was found.");
                return errorMessage;
            }

            student.Courses.Add(match);
            match.Students.Add(student);
            _context.SaveChanges();
            return errorMessage;
        }
    }
}
