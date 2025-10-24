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
using static System.Net.Mime.MediaTypeNames;

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

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "medicalrecords");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Save relative path to DB
            var relativePath = Path.Combine("medicalrecords", fileName).Replace("\\", "/");
         
            var records = new MedicalHistory
            {
                PatientId = dto.PatientId,
                Dtype=dto.Dtype,
                Tid= (int)dto.Tid,
                Records=relativePath,
                CreatedAt=dto.createdAt,

            };

            _context.MedicalHistories.Add(records);
            _context.SaveChanges();

            return new MedicalHistory
            {
                HistoryId=records.HistoryId,
                PatientId= records.PatientId,
                Dtype=records.Dtype,
                Records=relativePath,
                CreatedAt=records.CreatedAt,
                Tid = (int)records.Tid,

            };
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

            // FIX: Use the correct folder where files are saved
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "medicalrecords", fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found on disk at {filePath}");

            return File.ReadAllBytes(filePath);
        }
    }
}