using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Data;
using Models.DTOs;
using Models.Interfaces;
using Models.Models;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class AppointmentController : Controller
{
    private readonly IAppointmentServices _appointmentService;
    public AppointmentController(IAppointmentServices appointmentService)
    {        
        _appointmentService = appointmentService;
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
    public IActionResult AddAppointment([FromQuery]AppointmentDto dto)
    {
        if(dto == null) {
            return BadRequest("Invalid input");
        }
        var appointment = _appointmentService.Create(dto);
        return Ok(appointment);
    }

    [HttpPatch]
    [Route("/api/updateAppointment")]
    public IActionResult UpdateAppointment([FromQuery]int AppointmentId,[FromQuery]AppointmentDto dto)
    {
        if (AppointmentId == null)
        {
            return NotFound("Enter AppointmentId To Be Update");
        }

        _appointmentService.Update(AppointmentId,dto);
        return Ok("Appointment Id : "+AppointmentId +" has been updated.");

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
}
