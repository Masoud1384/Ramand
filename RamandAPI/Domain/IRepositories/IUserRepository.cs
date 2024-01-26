using Domain.Models;

namespace Domain.IRepositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        int Update(User user);
        bool Delete(int userId);
        // Also we can add a IsActive property to users so instead of delete we just deactivate the user
        int Create(User user);

        User GetUserBy(int id);
        User GetUserBy(string username);
    }
}
