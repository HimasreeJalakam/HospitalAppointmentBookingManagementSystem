using Infrastructure.DTOs;

using Infrastructure.Interfaces;

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

    //[Authorize]

    public class PersonController : ControllerBase

    {

        private readonly IPersonService _personServices;

        private readonly IConfiguration _configuration;

        public PersonController(IPersonService personServices, IConfiguration configuration)

        {

            _personServices = personServices;

            _configuration = configuration;

        }

        [HttpPost("login")]

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

        [HttpGet("GetAllPersons")]

        public IActionResult GetAllPersons()

        {

            var persons = _personServices.GetAllPersons();

            return Ok(persons);

        }

        [HttpGet("{id}")]

        public IActionResult GetPersonById(int id)

        {

            var person = _personServices.GetPersonById(id);

            if (person == null)

            {

                return NotFound();

            }

            return Ok(person);

        }
        [AllowAnonymous]
        [HttpPost("RegisterNewPerson")]

        public IActionResult AddPerson([FromQuery] PersonDto personDto)

        {

            var addedPerson = _personServices.Add(personDto);

            return Ok(addedPerson);

        }

        [HttpPut("{id}/UpdatePerson")]

        public IActionResult UpdatePerson(int id, [FromQuery] PersonDto personDto)

        {

            var updatedPerson = _personServices.Update(id, personDto);

            if (updatedPerson == null)

            {

                return NotFound();

            }

            return Ok(updatedPerson);

        }
        [HttpGet("GetByRole")]
        public IActionResult GetByRole([FromQuery] string role)
        {
            var rolePerson = _personServices.GetPersonDetailsByRole(role);
            return Ok(rolePerson);
        }


    }

}


