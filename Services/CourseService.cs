using System;
using System.Linq;
using StudentTimeTrackerApp.Models.Entities;
using StudentTimeTrackerApp.Entities;
using StudentTimeTrackerApp.Data;
using Microsoft.EntityFrameworkCore;

namespace StudentTimeTrackerApp.Services
{
    public class CourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a Course object. Can only be accessed by Instructors.
        /// </summary>
        /// <param name="userId">The Id of the currently logged in user.</param>
        /// <param name="courseCode">The code for the Course object (i.e., CSCI, HIST, etc.)</param>
        /// <param name="courseNum">The number of the Course object (i.e., 1120, 4250, etc.)</param>
        /// <param name="sectionNum">The section number of the Course object.</param>
        /// <returns>The error message, if there is one.</returns>
        public string CreateCourse(string userId, string courseCode, int courseNum, int sectionNum)
        {
            string errorMessage = string.Empty;
            var instructor = _context.Instructors
                .FirstOrDefault(i => i.UserId == userId);

            if (instructor == null) {
                errorMessage = "Instructor was not found. Please try again.";
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

        /// <summary>
        /// Finds and Adds a Student to a Course object. 
        /// </summary>
        /// <param name="userId">The Id of the currently logged in user.</param>
        /// <param name="courseCode">The code for the Course object (i.e., CSCI, HIST, etc.)</param>
        /// <param name="courseNum">The number of the Course object (i.e., 1120, 4250, etc.)</param>
        /// <param name="sectionNum">The section number of the Course object.</param>
        /// <returns>The error message, if there is one.</returns>
        public string FindCourse(string userId, string courseCode, int courseNum, int sectionNum)
        {
            string errorMessage = string.Empty;
            var student = _context.Students
                .FirstOrDefault(s => s.UserID == userId);

            if (student == null) {
                errorMessage = "No student found. Please try again.";
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
                return errorMessage;
            }

            student.Courses.Add(match);
            match.Students.Add(student);
            _context.SaveChanges();
            return errorMessage;
        }
        /// <summary>
        /// Finds all Courses taught by a specific Instructor.
        /// </summary>
        /// <param name="userId">The Id of the currently logged in user.</param
        /// <returns>A list of Courses taught by the Instructor.</returns>
        public List<Course>? FindCoursesByInstructor(string userId)
        {
            var instructor = _context.Instructors
                .FirstOrDefault(i => i.UserId == userId);

            if (instructor == null) {
                return null;
            }

            var courses = _context.Courses
                .Where(c => c.Instructors.Contains(instructor))
                .Include(c => c.Students)
                .Include(c => c.Instructors)
                .ToList();

            return courses;
        }

        /// <summary>
        /// Finds the Course By the Id number
        /// </summary>
        /// <param name="courseId">The Id of the Course object.</param>
        /// <returns>The Course object.</returns>
        public Course? GetCourseById(int courseId)
        {
            return _context.Courses
                .FirstOrDefault(c => c.Id == courseId);
        }

        /// <summary>
        /// Retrieves all the Students in a Course
        /// </summary>
        /// <param name="courseId">The Id of the Course object.</param>
        /// <returns>The Student objects as a List.</returns>
        public List<Student>? GetStudentsInCourse(int courseId)
        {
            var course = _context.Courses
                .FirstOrDefault(c => c.Id == courseId);

            if (course == null) {
                return null;
            }

            return (List<Student>?)course.Students;
        }
    }
}
