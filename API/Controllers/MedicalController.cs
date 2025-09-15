using Infrastructure.DTOs;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Data;
using Models.Helper;
using Models.Models;

namespace FileUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicalController : ControllerBase
    {
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        private readonly AppDbContext _context;
        private readonly MedicalHistoryService _history;
        public MedicalController(AppDbContext context,MedicalHistoryService history)
        {
            _context = context;
            _history = history;
            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        [HttpPost("AddMedicalHistory")]
        public async Task<IActionResult> UploadFile([FromQuery]MedicalHistoryDto dto,IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string filePath = Path.Combine(_uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }            

            MedicalHistory dtoEntity = new MedicalHistory
            {
                PatientId = dto.PatientId,
                Dtype = dto.Dtype,
                Tid = dto.Tid,
                Records =filePath,
                CreatedAt = DateTime.Now
            };

            _context.MedicalHistories.Add(dtoEntity);
            _context.SaveChanges();

            return Ok(new {dtoEntity} );
        }


        [HttpGet("DisplayMedicalHistory/{HistoryId}")]
        public IActionResult DisplayFile([FromQuery]int HistoryId)
        {
            var medicalHistory = _context.MedicalHistories.Find(HistoryId);
            string fileName = _history.getMedicalRecord(HistoryId);
            var filePath = Path.Combine(_uploadPath, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "application/octet-stream"; 

            var records = File(fileBytes, contentType, fileName);
            return Ok(new { medicalHistory } );
        }
    }
}
