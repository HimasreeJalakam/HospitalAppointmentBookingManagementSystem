using Microsoft.AspNetCore.Http;

namespace Infrastructure.DTOs;
public class MedicalHistoryDto
{
    public int PatientId { get; set; }
    public string Dtype { get; set; }
    public int? Tid { get; set; }
    public string records { get; set; }
    public DateTime createdAt { get; set; }
    public IFormFile File { get; set; }

}
