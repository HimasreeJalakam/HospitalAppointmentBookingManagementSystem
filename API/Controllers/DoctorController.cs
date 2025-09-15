using Infrastructure.DTOs;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorServices _doctorServices;
        public DoctorController(IDoctorServices doctorServices)
        {
            _doctorServices = doctorServices;
        }
        [HttpPost("{personId}/getSpeciality")]
        public IActionResult AddSpeciality(int personId, [FromQuery] SpecialityDto dto)
        {
            var speciality = _doctorServices.AddSpeciality(personId, dto);
            return Ok(speciality);
        }
        [HttpPut("{personId}/updateSpeciality")]
        public IActionResult UpdateSpeciality(int personId, [FromQuery] SpecialityDto dto)
        {
            var speciality = _doctorServices.UpdatingSpeciality(personId, dto);
            return Ok(speciality);
        }
    }
}
