using Infrastructure.DTOs;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Data;
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
            _context.Database.ExecuteSqlInterpolated(
                $"EXEC Doctor {personId}, {dto.Speciality}, {dto.YearsOfReg}"
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


    }
}
