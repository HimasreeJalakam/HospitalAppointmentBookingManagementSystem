namespace Models.Dto
{
    public class Treatmentdto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string Dtype { get; set; }
        public string Description { get; set; }
        public string Prescription { get; set; }
    }
}
