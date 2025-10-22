using StudentTimeTrackerApp.Data;
using StudentTimeTrackerApp.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTimeTrackerApp.Models.Entities
{
    public class Timecard
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required Student Student { get; set; }
    }
}
