using Infrastructure.DTOs;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Data;
using Models.Models;

namespace FileUpload.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MedicalController : ControllerBase
{
    private readonly IMedicalHistoryService _history;

    public MedicalController(IMedicalHistoryService history)
    {
        _history = history;
    }

    [HttpPost("AddMedicalHistory")]
    public async Task<IActionResult> AddMedicalHistory([FromForm] MedicalHistoryDto request)
    {
        var dto = new MedicalHistoryDto
        {
            PatientId = request.PatientId,
            Dtype = request.Dtype,
            Tid = request.Tid,
            createdAt = request.createdAt
        };

        var result = await _history.AddMedicalHistoryAsync(dto, request.File);
        return Ok(result);
    }
    [HttpGet("DisplayMedicalHistory")]
    public IActionResult DisplayFile([FromQuery] int PatientId)
    {
        try
        {
            var medicalHistory = _history.GetMedicalHistory(PatientId);
            if (medicalHistory == null)
                return NotFound("Medical history not found.");

            var fileBytes = _history.GetMedicalFileBytes(PatientId, out string fileName, out string contentType);
            return File(fileBytes, contentType, fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

