using Application.UserOperations.Commands;

namespace Application.UserOperations.IRepositoryApplication
{
    public interface IUserRepositoryApplication
    {
        IEnumerable<UserVM> GetAll();
        bool Update(UpdateUserCommand user);
        bool Delete(int userId);
        bool Create(CreateUserCommand user);

        UserVM GetUserBy(int id);
        UserVM GetUserBy(string username);
    }
}
