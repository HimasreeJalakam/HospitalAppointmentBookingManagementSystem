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

    [HttpGet]
    [Route("/api/getAppointmentsByPersonId/{PersonId}")]
    public IActionResult GetAppointmentsByPersonId(int PersonId)
    {
        var appointments = _appointmentService.GetByPersonId(PersonId);
        return Ok(appointments);
    }

    [HttpPost]
    [Route("/api/createAppointments")]
    public IActionResult AddAppointment([FromQuery]AppointmentDto dto)
    {
        if (dto.TimeSlotId == null || dto.AppointmentDate == null || dto.DoctorId == 0 || dto.PatientId == 0 || dto.Status == null)
        {
            return BadRequest("Invalid input");
        }
        _appointmentService.Create(dto);
        return Ok(dto);
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
}
