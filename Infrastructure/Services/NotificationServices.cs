using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Interfaces;
using Models.Models;

namespace Infrastructure.Services
{
    public class NotificationServices : INotificationServices
    {
        public readonly AppDbContext _context;
        private readonly EmailService _emailService;
        private readonly IPersonService _personServices;

        public NotificationServices(
            AppDbContext context,
            EmailService emailService,
            IPersonService personServices)
        {
            _context = context;
            _emailService = emailService;
            _personServices = personServices;
        }

        public List<Notification> GetAll()
        {
            return _context.Notifications.ToList();
        }

        public List<Notification> GetById(int personid)
        {
            var notifications = _context.Notifications
                .Where(a => a.DoctorId == personid || a.PatientId == personid)
                .ToList();
            return notifications;
        }

        public Notification Create(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();

            var patient = _personServices.GetPersonById(notification.PatientId);
            var doctor = _personServices.GetPersonById(notification.DoctorId);
            var appointment = _context.Appointments.FirstOrDefault(a => a.AppointmentId == notification.AppointmentId);
            var timeSlot = _context.TimeSlots.FirstOrDefault(ts => ts.TimeSlotId == appointment.TimeSlotId);

            string timeRange = timeSlot != null
                ? $"{timeSlot.StartTime:hh\\:mm tt} - {timeSlot.EndTime:hh\\:mm tt}"
                : "Unavailable";

            if (patient != null && !string.IsNullOrEmpty(patient.Email))
            {
                string subject = "Appointment Notification";
                string body = $"Dear {patient.FirstName},\n\nThis is regarding your upcoming appointment \n\n" +
                              $"{notification.Message} \n" +
                              $"Timings: {timeRange}\n\n" +
                              $"Regards,\nClinic Team";
                string body2 = $"Dear {doctor.FirstName},\n\nThis is regarding your upcoming appointment \n\n" +
                              $"{notification.Message} \n" +
                              $"Timings: {timeRange}\n\n" +
                              $"Regards,\nClinic Team";

                _emailService.SendEmail(patient.Email, subject, body);
                _emailService.SendEmail(doctor.Email, subject, body2);
            }

            return notification;
        }
    }
}
