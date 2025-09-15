using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AppointmentDto
    {
        public string? TimeSlotId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string? Status { get; set; }
    }
}
