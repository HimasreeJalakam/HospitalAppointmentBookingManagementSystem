using Models.Models;

namespace Infrastructure.Interfaces
{
    public interface ITreatmentDoneServices
    {
        public List<TreatmentDone> GetAll();
        public TreatmentDone GetById(int id);
        public TreatmentDone Create(TreatmentDone treatment);
    }
}
