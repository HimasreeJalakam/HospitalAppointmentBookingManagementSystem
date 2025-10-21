using Infrastructure.DTOs;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DoctorServices : IDoctorServices
    {
        private readonly AppDbContext _context;
        public DoctorServices(AppDbContext context)
        {
            _context = context;
        }
        public SpecialityDto AddSpeciality(int personId, SpecialityDto dto)
        {
            // Check if doctor already exists
            var existingDoctor = _context.Doctors.FirstOrDefault(d => d.PersonId == personId);
            if (existingDoctor != null)
                throw new Exception("This doctor already exists in the Doctor table.");

            // If not exists, call stored procedure to insert
            _context.Database.ExecuteSqlRaw(
                $"EXEC ListBasedOnDoctor {personId}, {dto.Speciality}, {dto.YearsOfReg}"
            );

            return dto;
        }
        public SpecialityDto UpdatingSpeciality(int personId, SpecialityDto dto)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.PersonId == personId);
            if (doctor != null)
            {
                doctor.Speciality = dto.Speciality;
                doctor.YearsOfReg = dto.YearsOfReg;
                _context.SaveChanges();
            }
            return dto;
        }
        public DoctorDto getBySpeciality(string speciality)
        {
            var doctors = _context.Doctors.Where(d => d.Speciality.ToLower() == speciality.ToLower())
                .Include(d => d.Person)
                .Select(d => new DoctorDto
                {
                    PersonId = (int)d.PersonId,
                    FirstName = d.Person.FirstName,
                    LastName = d.Person.LastName,
                    Dob = d.Person.Dob ?? default,
                    Speciality = d.Speciality,
                }).ToList();
            return doctors.FirstOrDefault();
        }

        public Doctor GetDoctorByPersonId(int personId)
        {
            var doctors = _context.Doctors.Where(d => d.PersonId == personId)
                .Include(d => d.Person)
                .Select(d => new Doctor
                {
                    DoctorId = d.DoctorId,
                    PersonId = d.PersonId,
                    Speciality = d.Speciality,
                    YearsOfReg = d.YearsOfReg,
                }).ToList();
            return doctors.FirstOrDefault();
        }

    }
}
