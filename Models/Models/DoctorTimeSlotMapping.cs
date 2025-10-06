using System;
using System.Collections.Generic;

namespace Models.Models;

public partial class DoctorTimeSlotMapping
{
    public int MappingId { get; set; }

    public int DoctorId { get; set; }

    public string? TimeSlotId { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    public int IsAvailable { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual TimeSlot? TimeSlot { get; set; }
}
