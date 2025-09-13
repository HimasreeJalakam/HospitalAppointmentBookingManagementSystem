using Microsoft.AspNetCore.Mvc;
using Models.Data;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AppointmentController : Controller
{
    private readonly AppDbContext _context;
    public AppointmentController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    [Route("/api/getAppointments")]
    public IActionResult GetAppointments()
    {
        var appointments = _context.Appointments.ToList();
        return Ok(appointments);
    }

    [HttpGet]
    [Route("/api/getAppointmentsByPersonId/{PersonId}")]
    public IActionResult GetAppointmentsByPersonId(int PersonId)
    {
        var appointments = _context.Appointments
            .Where(a => a.DoctorId == PersonId || a.PatientId == PersonId)
            .ToList();
        return Ok(appointments);
    }

    [HttpPost]
    [Route("/api/createAppointments")]
    public IActionResult AddAppointment(
        [FromQuery] string TimeSlotId,
        [FromQuery] DateOnly AppointmentDate,
        [FromQuery] int DoctorId,
        [FromQuery] int PatientId,
        [FromQuery] string Status
        )
    {
        if (TimeSlotId == null || AppointmentDate == null || DoctorId == 0 || PatientId == 0 || Status == null)
        {
            return BadRequest("Invalid input");
        }
        var newAppointment = new Models.Models.Appointment
        {
            TimeSlotId = TimeSlotId,
            AppointmentDate = AppointmentDate,
            DoctorId = DoctorId,
            PatientId = PatientId,
            Status = Status,
            CreatedAt = DateTime.Now,
            IsDeleted = "No"
        };
        _context.Appointments.Add(newAppointment);
        _context.SaveChanges();
        return Ok(newAppointment);
    }

    [HttpPatch]
    [Route("/api/updateAppointment")]
    public IActionResult UpdateAppointment(
        [FromQuery] int AppointmentId,
        [FromQuery] string TimeSlotId,
        [FromQuery] DateOnly AppointmentDate,
        [FromQuery] int DoctorId,
        [FromQuery] int PatientId,
        [FromQuery] string Status)
    {
        var AppointmentToUpdateId = _context.Appointments.Find(AppointmentId);
        if (AppointmentToUpdateId == null)
        {
            return NotFound("Enter AppointmentId To Be Update");
        }
        AppointmentToUpdateId.AppointmentId = AppointmentId;
        AppointmentToUpdateId.TimeSlotId = TimeSlotId;
        AppointmentToUpdateId.AppointmentDate = AppointmentDate;
        AppointmentToUpdateId.DoctorId = DoctorId;
        AppointmentToUpdateId.PatientId = PatientId;
        AppointmentToUpdateId.Status = Status;
        AppointmentToUpdateId.CreatedAt = DateTime.Now;
        AppointmentToUpdateId.IsDeleted = "No";

        _context.Update(AppointmentToUpdateId);
        _context.SaveChanges();
        return Ok(AppointmentToUpdateId);

    }
}
