using Infrastructure.DTOs;
using Models.Models;

namespace Infrastructure.Interfaces
{
    public interface IPersonService
    {
        bool ValidateUser(string username, string password);
        List<PersonDisplayDto> GetAllPersons();
        object GetPersonDetailsByRole(string role);
        PersonDisplayDto GetPersonById(int id);
        PersonDto Add(PersonDto personDto);

        PersonDto Update(int id, PersonDto person);


    }
}