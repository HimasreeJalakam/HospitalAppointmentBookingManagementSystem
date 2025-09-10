using System;
using System.Collections.Generic;

namespace Models.Models
{
    public partial class Notification
    {
        public int NotificationId { get; set; }

        public int? AppointmentId { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        public string? Message { get; set; }

        public DateTime? Timestamp { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual Appointment? Appointment { get; set; }

        public virtual Person Doctor { get; set; } = null!;

        public virtual Person Patient { get; set; } = null!;
    }
}
