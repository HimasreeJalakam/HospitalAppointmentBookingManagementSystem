using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.DTOs;
using Models.Interfaces;
using Models.Models;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AppointmentController : Controller
{
    private readonly IAppointmentServices _appointmentService;
    private readonly AppDbContext _context;
    public AppointmentController(IAppointmentServices appointmentService,AppDbContext context)
    {
        _appointmentService = appointmentService;
        _context = context;
    }
    [HttpGet]
    [Route("/api/getAppointments")]
    public IActionResult GetAppointments()
    {
        var appointments = _appointmentService.GetAll();
        return Ok(appointments);
    }

    [HttpGet("getAppointmentsByPersonId/{PersonId}")]
    //[Route("/api/getAppointmentsByPersonId/{PersonId}")]
    public IActionResult GetAppointmentsByPersonId(int PersonId)
    {
        var appointments = _appointmentService.GetByPersonId(PersonId);
        return Ok(appointments);
    }

    [HttpPost]
    [Route("/api/createAppointments")]
    public IActionResult AddAppointment([FromQuery] AppointmentDto dto)
    {
        if (dto == null) {
            return BadRequest("Invalid input");
        }
        var appointment = _appointmentService.Create(dto);
        return Ok(appointment);
    }

    [HttpPatch]
    [Route("/api/updateAppointment")]
    public IActionResult UpdateAppointment([FromQuery] int AppointmentId, [FromQuery] AppointmentDto dto)
    {
        if (AppointmentId == null)
        {
            return NotFound("Enter AppointmentId To Be Update");
        }

        _appointmentService.Update(AppointmentId, dto);
        return Ok("Appointment Id : " + AppointmentId + " has been updated.");

    }

    [HttpPut("cancelledAppointment/{appointmentId}")]
    public IActionResult CancelledAppointment(int appointmentId)
    {
        if (appointmentId == 0)
        {
            return BadRequest("Invalid AppointmentId");
        }
        _appointmentService.CancelledAppointment(appointmentId);
        return Ok("Appointment Id : " + appointmentId + " has been cancelled.");

    }
    [HttpPut("cancelAppointment/{appointmentId}")]
    public IActionResult CancelAppointment(int appointmentId)
    {
        try
        {
            var updatedAppointment = _appointmentService.CancelledAppointment(appointmentId);
            return Ok(new
            {
                message = "Appointment cancelled successfully",
                appointment = updatedAppointment
            });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    [HttpGet("Count")]
    public IActionResult GetCountAppointment()
    {
        var NoOfAppointments = _appointmentService.GetCountAppointment();
        return Ok(NoOfAppointments);
    }

    [HttpGet]
    [Route("/api/GetAppointmentsByDoctorId")]    
    public IActionResult GetAppointmentsByDoctorId(int doctorId)
    {
        var appointments = _appointmentService.GetAppointmentsByDoctorId(doctorId);
        return Ok(appointments);
    }
}
