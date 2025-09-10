using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Models;



namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class NotificationController : Controller
{
    private readonly AppDbContext _context;

    public NotificationController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllNotifications()
    {
        var notifications = _context.Notifications.ToList();
        return Ok(notifications);
    }
    

}


