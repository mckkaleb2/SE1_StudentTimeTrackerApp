using Microsoft.EntityFrameworkCore;
using StudentTimeTrackerApp.Data;
using StudentTimeTrackerApp.Entities;


namespace StudentTimeTrackerApp.Services
{
    public class InstructorService
    {
        private readonly ApplicationDbContext _context; // Add a field for the context

        public InstructorService(ApplicationDbContext context) // Inject the context via constructor
        {
            _context = context;
        }

        public void CreateInstructor(string firstName, string lastName, string aNum, string userId, ApplicationUser user)
        {
            var instructor = new Instructor // Create a new Instructor object
            {
                FirstName = firstName,
                LastName = lastName,
                ANum = aNum,
                UserId = userId,
                User = user
            };

            _context.Instructors.Add(instructor); // Add the instructor to the context
            _context.SaveChanges(); // Save changes to the database
        }

        public Instructor? GetInstructorByUserId(string userId)
        {
            return _context.Instructors.FirstOrDefault(i => i.UserId == userId);
        }

        public async Task<Instructor?> GetInstructorByUserIdAsync(string userId)
        {
            try { 
                return await _context.Instructors
                    .Where(s => s.UserId == userId)
                    .FirstOrDefaultAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

            

public bool UserIsInstructor(string userId)
        {
            return _context.Instructors.Any(i => i.UserId == userId);
        }
    }
}