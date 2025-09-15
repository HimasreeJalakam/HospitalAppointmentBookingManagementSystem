using Infrastructure.DTOs;
using Infrastructure.Interfaces;
using Models.Data;
using Models.Models;

namespace Infrastructure.Services
{
    public class PersonServices : IPersonService
    {
        private readonly AppDbContext _context;
        public PersonServices(AppDbContext context)
        {
            _context = context;
        }

        public bool ValidateUser(string username, string password)
        {
            var user = _context.People
                .FirstOrDefault(u => u.Email == username && u.Password == password);

            return user != null;
        }

        public List<Person> GetAllPersons()
        {
            return _context.People.ToList();
        }
        public Person? GetPersonById(int id)
        {
            return _context.People.FirstOrDefault(p => p.PersonId == id);
        }
        public PersonDto Add(PersonDto personDto)
        {
            var person = new Person
            {
                FirstName = personDto.FirstName,
                LastName = personDto.LastName,
                Dob = personDto.Dob,
                Gender = personDto.Gender,
                Address = personDto.Address,
                PhoneNo = personDto.PhoneNo,
                AltNo = personDto.AltNo,
                Email = personDto.Email,
                Role = personDto.Role,
                Password = personDto.Password,
                CreatedAt = DateTime.Now
            };
            _context.People.Add(person);

            _context.SaveChanges();
            var added = new PersonDto
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Dob = (DateOnly)person.Dob,
                Gender = person.Gender,
                Address = person.Address,
                PhoneNo = (long)person.PhoneNo,
                AltNo = (long)person.AltNo,
                Email = person.Email,
                Role = person.Role,
                Password = person.Password


            };
            return added;
        }
        public PersonDto Update(int id, PersonDto personDto)
        {
            var person = _context.People.FirstOrDefault(p => p.PersonId == id);
            if (person == null)
            {
                throw new Exception("Person not found");
            }
            person.FirstName = personDto.FirstName;
            person.LastName = personDto.LastName;
            person.Dob = personDto.Dob;
            person.Gender = personDto.Gender;
            person.Address = personDto.Address;
            person.PhoneNo = personDto.PhoneNo;
            person.AltNo = personDto.AltNo;
            person.Email = personDto.Email;
            person.Role = personDto.Role;
            person.Password = personDto.Password;
            _context.SaveChanges();

            var updated = new PersonDto
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Dob = (DateOnly)person.Dob,
                Gender = person.Gender,
                Address = person.Address,
                PhoneNo = (long)person.PhoneNo,
                AltNo = (long)person.AltNo,
                Email = person.Email,
                Role = person.Role,
                Password = person.Password
            };
            return updated;
        }
        public List<RoleDto> GetByRole(string role)
        {
            var persons = _context.People
                .Where(p => p.Role.ToLower() == role.ToLower())
                .Select(p => new RoleDto
                {
                    PersonId = p.PersonId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    PhoneNo = (long)p.PhoneNo,
                    Role = p.Role
                }).ToList();

            return persons;
        }

    }
}