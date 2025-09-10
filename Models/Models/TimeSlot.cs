using System;
using System.Collections.Generic;

namespace Models.ScaffoldModels
{
    public partial class TimeSlot
    {
        public string TimeSlotId { get; set; } = null!;

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public bool IsAvailable { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public virtual ICollection<DoctorTimeSlotMapping> DoctorTimeSlotMappings { get; set; } = new List<DoctorTimeSlotMapping>();
    }
}
