using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DbContext;
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
    public async Task<IActionResult> GetAllNotifications() =>
Ok(await _context.Notifications.ToListAsync());

}


