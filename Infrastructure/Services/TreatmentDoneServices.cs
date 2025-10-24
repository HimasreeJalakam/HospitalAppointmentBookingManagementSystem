using Infrastructure.Interfaces;
using Models.Data;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Models.Helper;

namespace Infrastructure.Services
{
    public class TreatmentDoneServices : ITreatmentDoneServices
    {
        private readonly AppDbContext _context;
        public TreatmentDoneServices(AppDbContext context)
        {
            _context = context;
        }
        public List<TreatmentDone> GetAll()
        {
            return _context.TreatmentDones.ToList();
        }
        public List<TreatmentDone> GetByPatientId(int patientId)
        {
            return _context.TreatmentDones
                           .Where(t => t.PatientId == patientId)
                           .ToList();
        }
        public TreatmentDone Create(TreatmentDone treatmentDone)
        {

            Dtype.Validate(treatmentDone.Dtype);
            _context.TreatmentDones.Add(treatmentDone);
            _context.SaveChanges();
            return treatmentDone;
        }
    }
}
