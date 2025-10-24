using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dto;
using Models.Models;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]

public class TreatmentDoneController : Controller
{
    private readonly ITreatmentDoneServices _treatmentDoneServices;

    public TreatmentDoneController(ITreatmentDoneServices treatmentservice)
    {
        _treatmentDoneServices = treatmentservice;
    }

    [HttpGet]
    [Route("/api/getAllTreatmentDone")]
    public IActionResult GetAllTreatmentDone()
    {
        return Ok(_treatmentDoneServices.GetAll());
    }

    [HttpGet]
    [Route("/api/getTreatmentDoneById/{personid}")]
    public ActionResult<TreatmentDone> GetTreatmentDoneByPersonId(int personid)
    {
        return Ok(_treatmentDoneServices.GetById(personid));
    }

    [HttpPost]
    [Route("/api/createTreatment")]
    public IActionResult AddTreatmentDone([FromQuery] Treatmentdto dto)
    {
        try
        {
            var newTreatment = new TreatmentDone
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                Dtype = dto.Dtype,
                Description = dto.Description,
                Prescription = dto.Prescription,
                FollowUp = dto.FollowUp,
                CreatedAt = DateTime.Now
            };

            _treatmentDoneServices.Create(newTreatment);
            return Ok(newTreatment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); 
        }
    }

}
