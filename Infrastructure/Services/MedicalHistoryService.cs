using Infrastructure.DTOs;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Data;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{

    public class MedicalHistoryService : IMedicalHistoryService
    {
        private readonly AppDbContext _context;
        private readonly string _uploadPath;

        public MedicalHistoryService(AppDbContext context)
        {
            _context = context;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        public async Task<MedicalHistory> AddMedicalHistoryAsync(MedicalHistoryDto dto, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            string filePath = Path.Combine(_uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var dtoEntity = new MedicalHistory
            {
                PatientId = dto.PatientId,
                Dtype = dto.Dtype,
                Tid = dto.Tid,
                Records = filePath,
                CreatedAt = DateTime.Now
            };

            _context.MedicalHistories.Add(dtoEntity);
            await _context.SaveChangesAsync();

            return dtoEntity;
        }

        public MedicalHistory GetMedicalHistory(int PatientId)
        {
            return _context.MedicalHistories.FirstOrDefault(h => h.PatientId == PatientId);
        }

        public byte[] GetMedicalFileBytes(int PatientId, out string fileName, out string contentType)
        {
            var history = GetMedicalHistory(PatientId);

            if (history == null || string.IsNullOrEmpty(history.Records))
                throw new FileNotFoundException("Medical record not found.");

            fileName = Path.GetFileName(history.Records);
            contentType = "application/octet-stream";

            var filePath = Path.Combine(_uploadPath, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found on disk.");

            return File.ReadAllBytes(filePath);
        }
    }
}
