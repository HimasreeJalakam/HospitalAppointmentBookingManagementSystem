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
        public IActionResult Login([FromBody] LoginDto loginRequest)
        {
            var user = _personServices.ValidateUser(loginRequest.Email, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            var jwtTokenGenerator = new TokenGeneration(_configuration);
            string tokenString = jwtTokenGenerator.GenerateJWT(
                personId: user.PersonId.ToString(),
                role: user.Role,
                name: user.FirstName + " " + user.LastName,
                email: user.Email
            );

            return Ok(new
            {
                Token = tokenString,
                PersonId = user.PersonId,
                Role = user.Role,
                Name = user.FirstName + " " + user.LastName,
                Email = user.Email
            });
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
        public IActionResult AddPerson([FromBody] PersonDto personDto)
        {
            var existingPerson = _personServices.GetPersonByEmail(personDto.Email);
            if (existingPerson != null)
            {
                return BadRequest("Email already exists");
            }

            var addedPerson = _personServices.Add(personDto);
            return Ok(new
            {
                personId = addedPerson.PersonId,
                role = addedPerson.Role
            });
        }

        [HttpPut("{id}/UpdatePerson")]
        public IActionResult UpdatePerson(int id, [FromBody] PersonDto personDto)
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

        [HttpGet("Count")]

        public IActionResult GetCount([FromQuery] string role)

        {

            var NoOfPersons = _personServices.GetCount(role);

            return Ok(NoOfPersons);

        }

        [HttpPut("updateDob")]

        public IActionResult UpdateDob(int personId, string newDob)

        {

            var person = _personServices.GetPersonById(personId);

            if (person == null)

                return NotFound("Person not found");

            // Create a PersonDto with only updated DOB and existing values

            var personDto = new PersonDto

            {

                FirstName = person.FirstName,

                LastName = person.LastName,

                Dob = DateOnly.Parse(newDob),

                Gender = person.Gender,

                Address = person.Address,

                PhoneNo = person.PhoneNo,

                AltNo = person.AltNo,

                Email = person.Email,

                Role = person.Role,

                Password = person.Password

            };

            var updatedPerson = _personServices.Update(personId, personDto);

            return Ok(updatedPerson);

        }

        [HttpGet("check-email")]

        public IActionResult CheckEmailExists(string email)

        {

            bool exists = _personServices.CheckEmailExistsAsync(email);

            return Ok(exists); // true if exists, false otherwise

        }

        [HttpGet("GetPersonByEmail")]

        public IActionResult GetPersonByEmail([FromQuery] string email)

        {

            var person = _personServices.GetPersonByEmail(email);

            if (person == null)

            {

                return NotFound("Email not found");

            }

            return Ok(person);

        }

        [HttpGet("test-exception")]

        public IActionResult ThrowException()

        {

            throw new Exception("This is a test exception.");

        }

    }

}


