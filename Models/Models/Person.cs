using System;
using System.Collections.Generic;

namespace Models.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public long? PhoneNo { get; set; }

    public long? AltNo { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Appointment> AppointmentDoctors { get; set; } = new List<Appointment>();

    public virtual ICollection<Appointment> AppointmentPatients { get; set; } = new List<Appointment>();

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();

    public virtual ICollection<Notification> NotificationDoctors { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationPatients { get; set; } = new List<Notification>();

    public virtual ICollection<TreatmentDone> TreatmentDoneDoctors { get; set; } = new List<TreatmentDone>();

    public virtual ICollection<TreatmentDone> TreatmentDonePatients { get; set; } = new List<TreatmentDone>();
}
