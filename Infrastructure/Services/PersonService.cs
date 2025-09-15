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
        
        public Person GetPersonById(int personId)
        {
            return _context.People.Find(personId);
        }

    }
}