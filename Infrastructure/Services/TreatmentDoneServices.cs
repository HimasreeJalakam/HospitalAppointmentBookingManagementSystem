using Infrastructure.Interfaces;
using Models.Data;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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
        public TreatmentDone? GetById(int personid)
        {
            return _context.TreatmentDones.Find(personid);
        }
        public TreatmentDone Create(TreatmentDone treatmentDone)
        {

            _context.TreatmentDones.Add(treatmentDone);
            _context.SaveChanges();
            return treatmentDone;
        }
    }
}
