using System.ComponentModel.DataAnnotations;

namespace StudentTimeTrackerApp.Models.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; } // I feel like this should be a GUID but oh well
        public string Body { get; set; } = string.Empty;
        // This will be the UserId of the sender grabbed with a GET before the message is sent.
        public string Sender { get; set; } = string.Empty;


        /// <summary>
        /// This will be the UserId of the receiver grabbed with a GET through the connection likely using the Course class.
        /// (NOTE: If Value is empty string, message is sent to all users in the course)
        /// </summary>
        public string Recipient { get; set; } = string.Empty;
        // Foreign key to associate message with a course
        public int CourseId { get; set; } = 0;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
