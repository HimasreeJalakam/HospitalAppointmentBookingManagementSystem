using System;
using System.Collections.Generic;

namespace Models.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public string? TimeSlotId { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? IsDeleted { get; set; }

    public virtual Person Doctor { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Person Patient { get; set; } = null!;

    public virtual TimeSlot? TimeSlot { get; set; }
}
