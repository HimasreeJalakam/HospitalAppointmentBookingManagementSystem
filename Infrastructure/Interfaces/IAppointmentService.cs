using Models.DTOs;
using Models.Models;

namespace Models.Interfaces
{
    public interface IAppointmentServices
    {
        List<Appointment> GetAll();
        List<Appointment> GetByPersonId(int personId);
        List<Appointment> GetAppointmentsByDate(DateOnly date);
        List<Appointment> GetAppointmentsByStatus(string status);
        public int GetCountAppointment();
        Appointment Create(AppointmentDto dto);
        Appointment Update(int appointmentId, AppointmentDto dto);
        Appointment CancelledAppointment(int appointmentId);
        Appointment FinishedAppointment(int appointmentId);
        void SoftDelete(int appointmentId);

    }
}
