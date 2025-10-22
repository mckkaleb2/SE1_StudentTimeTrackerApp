using StudentTimeTrackerApp.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTimeTrackerApp.Models.Entities
{
    public class Timecard
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Use a GET to grab the UserId when the Timecard is created to fill this field.
        public string UserId { get; set; } = string.Empty;
    }
}
