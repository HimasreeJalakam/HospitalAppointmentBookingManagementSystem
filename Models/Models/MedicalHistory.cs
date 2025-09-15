using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Models.Models;

public partial class MedicalHistory
{
    public int HistoryId { get; set; }

    public int PatientId { get; set; }

    public string? Dtype { get; set; }

    public int Tid { get; set; }

    public string Records { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Person Patient { get; set; } = null!;

    public virtual TreatmentDone TidNavigation { get; set; } = null!;
}
