namespace Infrastructure.Interfaces
{
    public interface IPersonService
    {
        bool ValidateUser(string username, string password);

    }
}