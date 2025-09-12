using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Models.Data;
using Models.DTOs;
using Models.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonController : ControllerBase
    {
        private readonly PersonServices _personServices;
        private readonly IConfiguration _configuration;
        public PersonController(PersonServices personServices, IConfiguration configuration)
        {
            _personServices = personServices;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromQuery] LoginDto loginRequest)
        {
            // Check DB Object and validate
            bool isValidUser = _personServices.ValidateUser(loginRequest.Email, loginRequest.Password);
            if (!isValidUser)
            {
                return Unauthorized("Invalid username or password.");
            }

            TokenGeneration jwtTokenString = new TokenGeneration(_configuration);
            string tokenString = jwtTokenString.GenerateJWT(loginRequest.Email, "Admin", "Asset", "All");

            return Ok(new { Token = tokenString });
        }
    }
}
