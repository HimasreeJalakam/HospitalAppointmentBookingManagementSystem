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
    [Route("/api/appointments")]
    public IActionResult GetAppointments()
    {
        var appointments = _context.Appointments.ToList();
        return Ok(appointments);
    }
}
