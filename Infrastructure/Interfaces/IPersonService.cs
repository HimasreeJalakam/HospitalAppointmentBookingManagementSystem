using Infrastructure.DTOs;
using Models.Models;

namespace Infrastructure.Interfaces
{
    public interface IPersonService
    {
        bool ValidateUser(string username, string password);
        List<Person> GetAllPersons();

        Person? GetPersonById(int id);
        PersonDto Add(PersonDto personDto);

        PersonDto Update(int id, PersonDto person);
       

    }
}