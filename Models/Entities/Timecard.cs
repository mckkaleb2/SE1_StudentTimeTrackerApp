using StudentTimeTrackerApp.Data;
using StudentTimeTrackerApp.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentTimeTrackerApp.Models.Entities
{
    public class Timecard
    {
        public int Id { get; set; }

        // InverseProperty ensures EF understands this is the principal side of the one-to-many relationship
        [InverseProperty(nameof(TimeEntry.Timecard))]
        public virtual ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();

        public required Student Student { get; set; }
    }
}
