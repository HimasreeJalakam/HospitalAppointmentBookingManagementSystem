using Models.Models;

namespace Infrastructure.Interfaces
{
    public interface ITreatmentDoneServices
    {
        public List<TreatmentDone> GetAll();
        public List<TreatmentDone> GetByPatientId(int patientId);
        public TreatmentDone Create(TreatmentDone treatment);
    }
}
