using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IDoctorServices
    {
        SpecialityDto AddSpeciality(int personId, SpecialityDto dto);
        SpecialityDto UpdatingSpeciality(int personId, SpecialityDto dto);
        public DoctorDto getBySpeciality(string speciality);
    }
}
