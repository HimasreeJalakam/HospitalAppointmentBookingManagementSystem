using Models.Data;
using Models.DTOs;
using Models.Interfaces;
using Models.Models;

namespace Infrastructure.Services
{
    public class AppointmentService : IAppointmentServices
    {
        private readonly AppDbContext _context;
        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public List<Appointment> GetAll()
        {
            return _context.Appointments.ToList();
        }
        public List<Appointment> GetByPersonId(int personId)
        {
            return _context.Appointments
                .Where(a => a.PatientId == personId || a.DoctorId == personId)
                .ToList();
        }
        public List<Appointment> GetAppointmentsByDate(DateOnly date)
        {
            return _context.Appointments
                .Where(a => a.AppointmentDate == date)
                .ToList();
        }
        public List<Appointment> GetAppointmentsByStatus(string status)
        {
            return _context.Appointments
                .Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        public Appointment Create(AppointmentDto dto)
        {
            var newAppointment = new Appointment
            {
                TimeSlotId = dto.TimeSlotId,
                AppointmentDate = dto.AppointmentDate,
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                Status = dto.Status,
                CreatedAt = DateTime.Now,
                IsDeleted = "No"
            };
            _context.Appointments.Add(newAppointment);
            _context.SaveChanges();
            return newAppointment;
        }
        public Appointment Update(int appointmentId, AppointmentDto dto)
        {
            var appointment = _context.Appointments.Find(appointmentId);
            if (appointment == null)
            {
                throw new Exception($"Appointment with ID {appointmentId} not found.");
            }
            appointment.TimeSlotId = dto.TimeSlotId ?? appointment.TimeSlotId;
            appointment.AppointmentDate = dto.AppointmentDate != default ? dto.AppointmentDate : appointment.AppointmentDate;
            appointment.DoctorId = dto.DoctorId != 0 ? dto.DoctorId : appointment.DoctorId;
            appointment.PatientId = dto.PatientId != 0 ? dto.PatientId : appointment.PatientId;
            appointment.Status = dto.Status ?? appointment.Status;
            _context.SaveChanges();
            return appointment;
        }
        public Appointment CancelledAppointment(int appointmentId)
        {
            var appointment = _context.Appointments.Find(appointmentId);
            if (appointment == null)
            {
                throw new Exception($"Appointment with ID {appointmentId} not found.");
            }
            appointment.Status = "Cancelled";
            _context.SaveChanges();
            return appointment;
        }
        public int GetCountAppointment()
        {
            return _context.Appointments.Count();
        }
        public Appointment FinishedAppointment(int appointmentId)
        {
            var appointment = _context.Appointments.Find(appointmentId);
            if (appointment == null)
            {
                throw new Exception($"Appointment with ID {appointmentId} not found.");
            }
            appointment.Status = "Finished";
            _context.SaveChanges();
            return appointment;
        }
        public void SoftDelete(int appointmentId)
        {
            var appointment = _context.Appointments.Find(appointmentId);
            if (appointment == null)
            {
                throw new Exception($"Appointment with ID {appointmentId} not found.");
            }
            appointment.IsDeleted = "Yes";
            _context.SaveChanges();
        }
    }
}
