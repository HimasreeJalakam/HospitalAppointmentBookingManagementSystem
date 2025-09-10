using System;
using System.Collections.Generic;

namespace Models.Models
{
    public partial class Doctor
    {
        public int PersonId { get; set; }

        public string? Speciality { get; set; }

        public int? YearsOfReg { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<DoctorTimeSlotMapping> DoctorTimeSlotMappings { get; set; } = new List<DoctorTimeSlotMapping>();

        public virtual Person Person { get; set; } = null!;
    }
}
