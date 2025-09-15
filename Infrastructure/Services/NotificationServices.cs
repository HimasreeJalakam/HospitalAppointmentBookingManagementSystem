using Infrastructure.Interfaces;
using Models.Data;
using Models.Models;
namespace Infrastructure.Services 
{
    public class NotificationServices : INotificationServices
    {
        public readonly AppDbContext _context;
        public NotificationServices(AppDbContext context)
        {
            _context = context;
        }
        public List<Notification> GetAll()
        {
            return _context.Notifications.ToList();
        }

        public  List<Notification> GetById(int personid)
        {
            var notifications = _context.Notifications.Where(a => a.DoctorId == personid || a.PatientId == personid).ToList();
            return notifications;
        }

        public Notification Create(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return notification;
        }
        
    }
}
