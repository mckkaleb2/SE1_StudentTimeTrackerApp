using Microsoft.EntityFrameworkCore;
using StudentTimeTrackerApp.Data; 
using StudentTimeTrackerApp.Entities; 

namespace StudentTimeTrackerApp.Services
{
    public class StudentService
    {
        private readonly ApplicationDbContext _context; // Add a field for the context

        public StudentService(ApplicationDbContext context) // Inject the context via constructor
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new Student object.
        /// </summary>
        /// <param name="firstName">The first name of the Student.</param>
        /// <param name="lastName">The last name of the Student.</param>
        /// <param name="aNum">The ANum (ENumber equivalent) of the Student.</param>
        /// <param name="userId">The UserId of the Student.</param>
        /// <param name="user">The user in EFCore.</param>
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

        /// <summary>
        /// Finds a particular Student object by its Id.
        /// </summary>
        /// <param name="userId">The Id of the User.</param>
        /// <returns>The Student object.</returns>
        public Student? GetStudentByUserId(string userId)
        {
            return _context.Students.FirstOrDefault(s => s.UserID == userId);
        }

        /// <summary>
        /// Finds a particular Student object by its Id asynchronously.
        /// </summary>
        /// <param name="userId">The Id of the User.</param>
        /// <returns>The the Student object.</returns>
        public async Task<Student?> GetStudentByUserIdAsync(string userId)
        {
            var waiter = _context.Students
                .Where(s => s.UserID == userId)
                .FirstOrDefaultAsync();
            await Task.WhenAll(waiter);
            return waiter.Result;
        }

        /// <summary>
        /// Confirms whether the User in EFCore is a Student.
        /// </summary>
        /// <param name="userId">The User Id in EFCore.</param>
        /// <returns>True or False.</returns>
        public bool UserIsStudent(string userId)
        {
            return _context.Students.Any(s => s.UserID == userId);
        }
    }
}