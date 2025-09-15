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
        public object GetPersonById(int id)
        {
            var person = _context.People.FirstOrDefault(p => p.PersonId == id);
            if (person == null)
                return null;

            return person;
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
        public List<object> GetByRole(string role)
        {
            role = role.ToLower();

            if (role == "doctor")
            {
                
var result = _context.People
    .Join(_context.Doctors,
        person => person.PersonId,
        doctor => doctor.PersonId,
        (person, doctor) => new
        {
            person.PersonId,
            Name = person.FirstName + " " + person.LastName,
            DoctorSpeciality = doctor != null ? doctor.Speciality : "None"
        })
    .ToList<object>();

                return result;
            }
            else if (role == "patient")
            {
                var patients = (from person in _context.People
                                join medicalhistory in _context.MedicalHistories
                                on person.PersonId equals medicalhistory.PatientId
                                where person.Role.ToLower() == "patient"
                                select new PatientDto
                                {
                                    PersonId = person.PersonId,
                                    FirstName = person.FirstName,
                                    LastName = person.LastName,
                                    Email = person.Email,
                                    PhoneNo = (long)person.PhoneNo,
                                    Dtype = medicalhistory.Dtype,
                                    Records = medicalhistory.Records
                                }).ToList<object>();

                return patients;
            }

            return new List<object>(); // Return empty if role doesn't match
        }

        Person? IPersonService.GetPersonById(int id)
        {
            throw new NotImplementedException();
        }
    }
}