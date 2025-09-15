using Infrastructure.Interfaces;
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
        public MedicalHistoryService(AppDbContext context)
        {
            _context = context;
        }

        public string getMedicalRecord(int HistoryId)
        {
            var h =_context.MedicalHistories
                .Where(m => m.HistoryId == HistoryId)
                .Select(m => Path.GetFileName(m.Records))
                .FirstOrDefault();
            return h;
        }

    }
}
