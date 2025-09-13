using System;
using System.Collections.Generic;

namespace Models.Models;

public partial class TreatmentDone
{
    public int Tid { get; set; }

    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public string? Dtype { get; set; }

    public string? Description { get; set; }

    public string? Prescription { get; set; }

    public DateTime? FollowUp { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Person Doctor { get; set; } = null!;

    public virtual ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();

    public virtual Person Patient { get; set; } = null!;
}
