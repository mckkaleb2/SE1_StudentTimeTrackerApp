using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTimeTrackerApp.Models.Entities
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Foreign key for Timecard
        public int TimecardId { get; set; }
        
        // Navigation property
        [ForeignKey("TimecardId")]
        public required Timecard Timecard { get; set; }
    }
}