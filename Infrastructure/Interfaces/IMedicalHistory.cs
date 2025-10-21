using Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using Models.Models;

namespace Infrastructure.Interfaces;
public interface IMedicalHistoryService
{

    Task<MedicalHistory> AddMedicalHistoryAsync(MedicalHistoryDto dto, IFormFile file);
    MedicalHistory GetMedicalHistory(int historyId);
    byte[] GetMedicalFileBytes(int historyId, out string fileName, out string contentType);

}
