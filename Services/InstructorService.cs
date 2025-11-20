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

        /// <summary>
        /// Creates a new Instructor object.
        /// </summary>
        /// <param name="firstName">The first name of the Instructor.</param>
        /// <param name="lastName">The last name of the Instructor.</param>
        /// <param name="aNum">The ANum (ENumber equivalent) of the Instructor.</param>
        /// <param name="userId">The Id of the user.</param>
        /// <param name="user">The user in EFCore.</param>
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

        /// <summary>
        /// Finds a particular Instructor object by the Id.
        /// </summary>
        /// <param name="userId">The Id of the User.</param>
        /// <returns>The Instructor object.</returns>
        public Instructor? GetInstructorByUserId(string userId)
        {
            return _context.Instructors.FirstOrDefault(i => i.UserId == userId);
        }

        /// <summary>
        /// Finds a particular Instructor object by the Id asynchronously.
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <returns>The Instructor object.</returns>
        public async Task<Instructor?> GetInstructorByUserIdAsync(string userId)
        {
            return await _context.Instructors
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Determines whether a user is an Instructor or not.
        /// </summary>
        /// <param name="userId">The Id of the EFCore user.</param>
        /// <returns>True or False.</returns>
        public bool UserIsInstructor(string userId)
        {
            return _context.Instructors.Any(i => i.UserId == userId);
        }
    }
}