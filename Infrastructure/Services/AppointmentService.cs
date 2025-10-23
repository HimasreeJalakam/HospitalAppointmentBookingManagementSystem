using Microsoft.EntityFrameworkCore;
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
            var conflictingAppointment = _context.Appointments.FirstOrDefault(a =>
                a.DoctorId == dto.DoctorId &&
                a.AppointmentDate == dto.AppointmentDate &&
                a.TimeSlotId == dto.TimeSlotId &&
                a.Status != "Cancelled" &&
                a.IsDeleted != "Yes"
            );

            if (conflictingAppointment != null)
            {
                throw new Exception($"Time slot already booked for DoctorId {dto.DoctorId} on {dto.AppointmentDate:yyyy-MM-dd} at TimeSlotId {dto.TimeSlotId}.");
            }

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

            var doctorNotification = new Notification
            {
                AppointmentId = newAppointment.AppointmentId,
                DoctorId = newAppointment.DoctorId,
                PatientId = newAppointment.PatientId,
                Message = "Appointment Created",
                CreatedAt = DateTime.Now
            };

            var patientNotification = new Notification
            {
                AppointmentId = newAppointment.AppointmentId,
                DoctorId = newAppointment.DoctorId,
                PatientId = newAppointment.PatientId,
                Message = "Appointment Created",
                CreatedAt = DateTime.Now
            };

            _context.Notifications.Add(doctorNotification);
            _context.Notifications.Add(patientNotification);
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


            var newTimeSlotId = dto.TimeSlotId ?? appointment.TimeSlotId;
            var newAppointmentDate = dto.AppointmentDate != default ? dto.AppointmentDate : appointment.AppointmentDate;
            var newDoctorId = dto.DoctorId != 0 ? dto.DoctorId : appointment.DoctorId;

            // Check for booking conflict (excluding current appointment)
            bool isConflict = _context.Appointments.Any(a =>
                a.AppointmentId != appointmentId &&
                a.DoctorId == newDoctorId &&
                a.AppointmentDate == newAppointmentDate &&
                a.TimeSlotId == newTimeSlotId &&
                a.Status != "Cancelled" &&
                a.IsDeleted != "Yes"
            );

            if (isConflict)
                throw new Exception("This time slot is already booked for the selected doctor.");


            appointment.TimeSlotId = dto.TimeSlotId ?? appointment.TimeSlotId;
            appointment.AppointmentDate = dto.AppointmentDate != default ? dto.AppointmentDate : appointment.AppointmentDate;
            appointment.DoctorId = dto.DoctorId != 0 ? dto.DoctorId : appointment.DoctorId;
            appointment.PatientId = dto.PatientId != 0 ? dto.PatientId : appointment.PatientId;
            appointment.Status = dto.Status ?? appointment.Status;
            _context.Appointments.Update(appointment);
            _context.SaveChanges();

            var doctorNotification = new Notification
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                Message = "Appointment Created",
                CreatedAt = DateTime.Now
            };
            var patientNotification = new Notification
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                Message = "Appointment Created",
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(doctorNotification);
            _context.Notifications.Add(patientNotification);
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
            var doctorNotification = new Notification
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                Message = "Appointment Created",
                CreatedAt = DateTime.Now
            };
            var patientNotification = new Notification
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                Message = "Appointment Created",
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(doctorNotification);
            _context.Notifications.Add(patientNotification);
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
        public List<object> GetAppointmentsByDoctorId(int doctorId)
        {
            var appointments = _context.Appointments
            .Where(a => a.DoctorId == doctorId)
            .Include(a => a.Patient)
            .Select(a => new
            {
                a.AppointmentId,
                a.AppointmentDate,
                a.TimeSlotId,
                a.Status,
                a.Patient.FirstName,
                a.Patient.LastName
            })
            .ToList<object>();
            return appointments;
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
