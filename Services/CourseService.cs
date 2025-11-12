using Microsoft.EntityFrameworkCore;
using StudentTimeTrackerApp.Data;
using StudentTimeTrackerApp.Entities;
using StudentTimeTrackerApp.Models.Entities;

namespace StudentTimeTrackerApp.Services;

public class CourseService
{
    private readonly ApplicationDbContext _context; // Add a field for the context

    public CourseService(ApplicationDbContext context) // Inject the context via constructor
    {
        _context = context;
    }

    public void CreateCourse(int id, string CourseCode, int CourseNum, int SectionNum, ICollection<Student> students, ICollection<Instructor> instructors)
    {
        var course = new Course // Create a new Instructor object
        {
            Id = id,
            CourseCode = CourseCode,
            CourseNum = CourseNum,
            SectionNum = SectionNum,
            Students = students,
            Instructors = instructors
        };
        _context.Courses.Add(course); // Add the instructor to the context
        _context.SaveChanges(); // Save changes to the database
    }

    public Course? GetCourseByID(int id)
    {
        return _context.Courses.FirstOrDefault(i => i.Id == id);
    }
    public async Task<Course?> AsyncGetCourseByID(int id)
    {
        return await _context.Courses.FirstOrDefaultAsync(i => i.Id == id);
    }

}