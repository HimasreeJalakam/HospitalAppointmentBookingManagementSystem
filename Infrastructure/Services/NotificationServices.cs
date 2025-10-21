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
        private readonly IAppointmentServices _appointmentService;
        public NotificationServices(
            AppDbContext context,
            EmailService emailService,
            IPersonService personServices,
            IAppointmentServices appointmentService
            )
        {
            _context = context;
            _emailService = emailService;
            _personServices = personServices;
            _appointmentService = appointmentService;
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

            var appointment = _appointmentService.GetAll().FirstOrDefault(a => a.AppointmentId == notification.AppointmentId);

            var timeSlot = _context.TimeSlots.FirstOrDefault(ts => ts.TimeSlotId == appointment.TimeSlotId);

            string timeRange = $"{timeSlot.StartTime.ToString("hh\\:mm tt")} - {timeSlot.EndTime.ToString("hh\\:mm tt")}";

            if (patient != null && !string.IsNullOrEmpty(patient.Email))
            {
                string subject = "Appointment Notification";
                string body = $"Dear {patient.FirstName},\n\nThis is regarding your upcoming appointment \n\n {notification.Message} \n\n Appointment ID : {notification.AppointmentId} \n Doctor ID : {notification.DoctorId} \n Appointment Date : {appointment?.AppointmentDate}\n Timings: {timeRange}\n\nRegards,\nClinic Team";
                _emailService.SendEmail(patient.Email, subject, body);
            }
            return notification;
        }
    }
}
