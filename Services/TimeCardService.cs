using System;
using System.Linq;
using StudentTimeTrackerApp.Models.Entities;
using StudentTimeTrackerApp.Data;

namespace Services
{
    /// <summary>
    /// Service for managing time cards and time entries for students
    /// </summary>
    public class TimeCardService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the TimeCardService
        /// </summary>
        /// <param name="context">The database context</param>
        public TimeCardService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new time entry for a student, associating it with their current timecard or creating a new one
        /// </summary>
        /// <param name="userId">The student's ASP.NET Identity user ID (GUID)</param>
        /// <param name="startTime">The clock-in time</param>
        /// <param name="latitude">The latitude coordinate of the clock-in location</param>
        /// <param name="longitude">The longitude coordinate of the clock-in location</param>
        /// <exception cref="InvalidOperationException">Thrown when student is not found</exception>
        public int CreateTimeEntry(string userId, DateTime startTime, double latitude, double longitude)
        {
            // Get the latest timecard for the employee
            var latestTimecard = _context.Timecards
                .Where(t => t.Student.UserID == userId)
                .FirstOrDefault();

            var student = _context.Students
                .FirstOrDefault(s => s.UserID == userId);

            if (student == null)
            {
                throw new InvalidOperationException("Student not found");
            }

            // Create new timecard if none exists or if latest is closed
            var timecard = latestTimecard ?? new Timecard
            {
                Student = student
            };

            var timeEntry = new TimeEntry
            {
                TimeIn = startTime,
                Timecard = timecard,
                Latitude = latitude,
                Longitude = longitude
            };

            if (timecard.Id == 0)
            {
                _context.Timecards.Add(timecard);
            }
            _context.TimeEntries.Add(timeEntry);
            _context.SaveChanges();
            return timeEntry.Id;
        }

        /// <summary>
        /// Closes an existing time entry by setting its end time
        /// </summary>
        /// <param name="timeEntryId">The ID of the time entry to close</param>
        /// <param name="endTime">The clock-out time</param>
        /// <remarks>
        /// If the time entry is not found, the method will silently return without making any changes
        /// </remarks>
        public void CloseTimeEntry(int timeEntryId, DateTime endTime)
        {
            var timeEntry = _context.TimeEntries.Find(timeEntryId);
            if (timeEntry != null)
            {
                timeEntry.TimeOut = endTime;
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves all time entries for a specific student
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of all time entries for student</returns>
        public List<TimeEntry> GetTimeEntriesForStudent(string userId)
        {
            return _context.TimeEntries
                .Where(te => te.Timecard.Student.UserID == userId)
                .ToList();
        }
    }
}