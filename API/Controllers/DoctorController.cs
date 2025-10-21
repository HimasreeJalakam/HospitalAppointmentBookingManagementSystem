using Infrastructure.DTOs;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorServices _doctorServices;
        public DoctorController(IDoctorServices doctorServices)
        {
            _doctorServices = doctorServices;
        }
        [HttpPost("addSpeciality/{personId}")]
        public IActionResult AddSpeciality(int personId, [FromBody] SpecialityDto dto)
        {
            try
            {
                var result = _doctorServices.AddSpeciality(personId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("{personId}/updateSpeciality")]
        public IActionResult UpdateSpeciality(int personId, [FromQuery] SpecialityDto dto)
        {
            var speciality = _doctorServices.UpdatingSpeciality(personId, dto);
            return Ok(speciality);
        }
        [HttpGet("GetBySpeciality")]
        public IActionResult GetBySpeciality([FromQuery] string speciality)
        {
            var doctor = _doctorServices.getBySpeciality(speciality);
            return Ok(doctor);
        }
    }
}
