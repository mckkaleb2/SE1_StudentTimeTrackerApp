using StudentTimeTrackerApp.Data; 
using StudentTimeTrackerApp.Entities; 

namespace Services
{
    public class StudentService
    {
        private readonly ApplicationDbContext _context; // Add a field for the context

        public StudentService(ApplicationDbContext context) // Inject the context via constructor
        {
            _context = context;
        }

        public void CreateStudent(string firstName, string lastName, string aNum, string userId, ApplicationUser user)
        {
            var student = new Student // Create a new Student object
            {
                FirstName = firstName,
                LastName = lastName,
                ANum = aNum,
                UserID = userId,
                User = user
            };

            _context.Students.Add(student); // Add the student to the context
            _context.SaveChanges(); // Save changes to the database
        }

        public Student GetStudentByUserId(string userId)
        {
            return _context.Students.FirstOrDefault(s => s.UserID == userId);
        }

        public bool userIsStudent(string userId)
        {
            return _context.Students.Any(s => s.UserID == userId);
        }
    }
}