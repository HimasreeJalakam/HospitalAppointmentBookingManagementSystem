using Models.Models;

namespace Infrastructure.Interfaces
{
    public interface INotificationServices
    {
        public List<Notification> GetAll();
        public List<Notification> GetById(int id);
        public Notification Create(Notification notification);
    }
}
