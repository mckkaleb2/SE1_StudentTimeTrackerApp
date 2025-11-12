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

        public void CreateCourse(string userId, string courseCode, int courseNum, int sectionNum)
        {
            var instructor = _context.Instructors
                .FirstOrDefault(i => i.UserId == userId);

            if (instructor == null) {
                throw new InvalidOperationException("Instructor not found.");
            }

            var course = new Course
            {
                CourseCode = courseCode,
                CourseNum = courseNum,
                SectionNum = sectionNum,
            };

            course.Instructors.Add(instructor);
        }
    }
}
