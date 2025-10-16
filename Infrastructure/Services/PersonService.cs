using Infrastructure.DTOs;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        public List<PersonDisplayDto> GetAllPersons()
        {
            var peoples = _context.People
                .Select(p => new PersonDisplayDto
                {
                    PersonId = p.PersonId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Dob = p.Dob ?? default,
                    Gender = p.Gender,
                    Address = p.Address,
                    PhoneNo = p.PhoneNo ?? 0,
                    AltNo = p.AltNo ?? 0,
                    Email = p.Email,
                    Role = p.Role,
                }).ToList();

            return peoples;
        }
        public PersonDisplayDto GetPersonById(int id)
        {
            var person = _context.People.FirstOrDefault(p => p.PersonId == id);
            if (person != null)
            {
                return new PersonDisplayDto
                {
                    PersonId = person.PersonId,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Dob = (DateOnly)person.Dob,
                    Email = person.Email,
                    Gender = person.Gender,
                    Address = person.Address,
                    PhoneNo = (long)person.PhoneNo,
                    AltNo = (long)person.AltNo,
                    Role = person.Role

                };
            }
            return null;
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

            {   PersonId=person.PersonId,
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
        public object GetPersonDetailsByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return null;
            var normalizedRole = role.Trim().ToLower();
            if (normalizedRole == "doctor")
            {
                var doctors = _context.People
                              .Include(p => p.Doctors)
                              .Where(p => p.Role != null && p.Role.Trim().ToLower() == "doctor")
                              .Select(p => new DoctorDto
                              {
                                  PersonId = p.PersonId,
                                  FirstName = p.FirstName,
                                  LastName = p.LastName,
                                  Email = p.Email,
                                  Gender = p.Gender,
                                  Dob = p.Dob.HasValue ? p.Dob.Value : default,
                                  PhoneNo = p.PhoneNo.HasValue ? p.PhoneNo.Value : 0,
                                  Address = p.Address,
                                  AltNo = p.AltNo.HasValue ? p.AltNo.Value : 0,
                                  Speciality = p.Doctors.Select(d => d.Speciality).FirstOrDefault(),
                                  YearsOfReg = p.Doctors.Select(d => d.YearsOfReg.HasValue ? d.YearsOfReg.Value : 0).FirstOrDefault()
                              })
                            .ToList();
                return doctors;
            }
            else if (normalizedRole == "patient")
            {
                var patients = _context.People
                               .Include(p => p.MedicalHistories)
                               .Where(p => p.Role != null && p.Role.Trim().ToLower() == "patient")
                               .Select(p => new PatientDto
                                            {
                                PersonId = p.PersonId,
                                FirstName = p.FirstName,
                                LastName = p.LastName,
                                Dob = p.Dob.HasValue ? p.Dob.Value : default,
                                PhoneNo = p.PhoneNo.HasValue ? p.PhoneNo.Value : 0,
                                Address = p.Address,
                                AltNo = p.AltNo.HasValue ? p.AltNo.Value : 0,
                                Email = p.Email,
                                Gender = p.Gender,
                                Dtype = p.MedicalHistories.Select(m => m.Dtype).FirstOrDefault(),
                                Records = p.MedicalHistories.Select(m => m.Records).FirstOrDefault()
                            })
                            .ToList();
                return patients;
            }
            else if (normalizedRole == "staff")
            {
                var staff = _context.People
                    .Where(p => p.Role != null && p.Role.Trim().ToLower() == "staff")
                    .Select(p => new StaffDto
                    {
                        PersonId = p.PersonId,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Dob = p.Dob.HasValue ? p.Dob.Value : default,
                        Gender = p.Gender,
                        PhoneNo = p.PhoneNo.HasValue ? p.PhoneNo.Value : 0,
                        AltNo = p.AltNo.HasValue ? p.AltNo.Value : 0,
                        Email = p.Email,
                        Address = p.Address
                    })
                    .ToList();
                return staff;
            }
            return null;
        }
        public int GetCount(string role)
        {
            return _context.People.Count(p => p.Role.ToLower() == role.ToLower()); 
        }       
    }
}