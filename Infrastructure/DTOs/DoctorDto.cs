using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class DoctorDto
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateOnly Dob { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public long PhoneNo { get; set; }

        public long AltNo { get; set; }

        public string Email { get; set; }

        public string Speciality { get; set; }

        public int YearsOfReg { get; set; }

    }
}
