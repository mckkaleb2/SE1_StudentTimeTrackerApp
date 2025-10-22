using System.ComponentModel.DataAnnotations;

namespace StudentTimeTrackerApp.Models.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; } = string.Empty;
        // This will be the UserId of the sender grabbed with a GET before the message is sent.
        public string Sender { get; set; } = string.Empty;
        // This will be the UserId of the receiver grabbed with a GET through the connection likely using the Course class.
        public string Recipient { get; set; } = string.Empty;
    }
}
