using Infrastructure.DTOs;

using Models.Models;

namespace Infrastructure.Interfaces

{

    public interface IPersonService

    {

        Person ValidateUser(string username, string password);

        List<PersonDisplayDto> GetAllPersons();

        public PersonDto GetPersonByEmail(string email);

        public bool CheckEmailExistsAsync(string email);

        object GetPersonDetailsByRole(string role);

        PersonDisplayDto GetPersonById(int id);

        PersonDto Add(PersonDto personDto);

        public int GetCount(string role);

        PersonDto Update(int id, PersonDto person);

    }

}
