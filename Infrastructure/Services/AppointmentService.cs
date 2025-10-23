using Infrastructure.Interfaces;
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
        private readonly INotificationServices _notificationService;

        public AppointmentService(AppDbContext context, INotificationServices notificationService)
        {
            _context = context;
            _notificationService = notificationService;
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

            var notification = new Notification
            {
                AppointmentId = newAppointment.AppointmentId,
                DoctorId = newAppointment.DoctorId,
                PatientId = newAppointment.PatientId,
                Message = $"Your Appointment is Scheduled \nDetails :\n\nAppointment ID : {newAppointment.AppointmentId}\nPatientID : {newAppointment.PatientId}\nDoctorID : {newAppointment.DoctorId}\nAppointment Date : {dto.AppointmentDate}",
                CreatedAt = DateTime.Now
            };  

            _notificationService.Create(notification);
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

            appointment.TimeSlotId = newTimeSlotId;
            appointment.AppointmentDate = newAppointmentDate;
            appointment.DoctorId = newDoctorId;
            appointment.PatientId = dto.PatientId != 0 ? dto.PatientId : appointment.PatientId;
            appointment.Status = dto.Status ?? appointment.Status;

            _context.Appointments.Update(appointment);
            _context.SaveChanges();

            var notification = new Notification
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                Message = "Appointment Updated",
                CreatedAt = DateTime.Now
            };

            _notificationService.Create(notification);
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

            var notification = new Notification
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                Message = $"Your Appointment is Cancelled \nDetails :\n\nAppointment ID : {appointment.AppointmentId}\nPatientID : {appointment.PatientId}\nDoctorID : {appointment.DoctorId}",
                CreatedAt = DateTime.Now
            };

            _notificationService.Create(notification);
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
