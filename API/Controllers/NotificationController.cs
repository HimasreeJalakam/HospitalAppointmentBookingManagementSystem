using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Models;



namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationController : Controller
{
    private readonly AppDbContext _context;

    public NotificationController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("all notifications")]
    public IActionResult GetAllNotifications()
    {
        var notifications = _context.Notifications.ToList();
        return Ok(notifications);
    }


    [HttpGet]
    [Route("/api/getNotificationsByNotificationId/{NotificationId}")]
    public ActionResult<Notification> GetNotificationById(int NotificationId)
    {
        var notification = _context.Notifications.Find(NotificationId);
        if (notification == null)
        {
            return NotFound($"Notification with ID {NotificationId} not found.");
        }
        return Ok(notification);
    }
    
    [HttpPost]
    [Route("/api/createNotification")]
    public IActionResult AddNotification(
    [FromQuery] int AppointmentId,
    [FromQuery] int DoctorId,
    [FromQuery] int PatientId,
    [FromQuery] string Message)
    {
        if (AppointmentId == 0 || DoctorId == 0 || PatientId == 0 || string.IsNullOrEmpty(Message))
        {
            return BadRequest("Invalid input");
        }

        var newNotification = new Notification
        {
            AppointmentId = AppointmentId,
            DoctorId = DoctorId,
            PatientId = PatientId,
            Message = Message,
            CreatedAt = DateTime.Now
        };
        _context.Notifications.Add(newNotification);
        _context.SaveChanges();

        return Ok(newNotification);
    }

    [HttpPatch]
    [Route("/api/updateNotification")]
    public IActionResult UpdateNotification(
    [FromQuery] int NotificationId,
    [FromQuery] int AppointmentId,
    [FromQuery] int DoctorId,
    [FromQuery] int PatientId,
    [FromQuery] string Message)
    {
        var notificationToUpdate = _context.Notifications.Find(NotificationId);
        if (notificationToUpdate == null)
        {
            return NotFound("Enter NotificationId to be updated");
        }

        notificationToUpdate.AppointmentId = AppointmentId;
        notificationToUpdate.DoctorId = DoctorId;
        notificationToUpdate.PatientId = PatientId;
        notificationToUpdate.Message = Message;
        // Keep CreatedAt unchanged

        _context.Update(notificationToUpdate);
        _context.SaveChanges();

        return Ok(notificationToUpdate);
    }


}
