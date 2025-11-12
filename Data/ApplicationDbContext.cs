using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentTimeTrackerApp.Models.Entities;
using StudentTimeTrackerApp.Entities;

namespace StudentTimeTrackerApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Timecard> Timecards { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<Message> Messages { get; set; }
        }
}
