using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Dto;
using Models.Models;



namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationController : Controller
{
    private readonly INotificationServices _notificationservice;

    public NotificationController(INotificationServices notificationservice, PersonServices personServices)
    {
        _notificationservice = notificationservice;
    }

    [HttpGet("all notifications")]
    public IActionResult GetAllNotifications()
    {
        var notifications = _notificationservice.GetAll();
        return Ok(notifications);
    }


    [HttpGet]
    [Route("/api/getNotificationsByPersonId/{personId}")]
    public ActionResult<Notification> GetNotificationById(int personId)
    {
        var notification = _notificationservice.GetById(personId);
        if (notification == null)
        {
            return NotFound($"Notification with ID {personId} not found.");
        }
        return Ok(notification);
    }

    [HttpPost]
    [Route("/api/createNotification")]
    public IActionResult AddNotification([FromQuery] NotificationDto dto)
    {
        var newNotification = new Notification
        {
            AppointmentId = dto.AppointmentId,
            DoctorId = dto.DoctorId,
            PatientId = dto.PatientId,
            Message = dto.Message,
            CreatedAt = DateTime.Now
        };
        _notificationservice.Create(newNotification);
        return Ok(newNotification);
    }

}