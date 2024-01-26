using Application.UserOperations.Commands;
using Application.UserOperations.IRepositoryApplication;
using Domain.IRepositories;
using Domain.Models;

namespace Application.UserOperations.RepositoryApplication
{
    public class UserRepositoryApplication : IUserRepositoryApplication
    {
        private IUserRepository _userRepository;

        public UserRepositoryApplication(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool Create(CreateUserCommand createUserCommand)
        {
            if (!(string.IsNullOrWhiteSpace(createUserCommand.username) && string.IsNullOrWhiteSpace(createUserCommand.password)))
            {
                var user = new Domain.Models.User(createUserCommand.username, createUserCommand.password);
                var result = _userRepository.Create(user);
                return result > 0;
            }
            return false;
        }

        public bool Delete(int userId)
        {
            if (userId > 0)
            {
                var result = _userRepository.Delete(userId);
                return result > 0;
            }
            return false;
        }

        public IEnumerable<UserVM> GetAll()
        {
            return (List<UserVM>)_userRepository.GetAll();
        }

        public UserVM GetUserBy(int id)
        {
            return (UserVM)_userRepository.GetUserBy(id);
        }

        public UserVM GetUserBy(string username)
        {
            return (UserVM)_userRepository.GetUserBy(username);
        }

        public bool Update(UpdateUserCommand user)
        {
            if (user.Id > 0)
            {
                var result = _userRepository.Update(new User(user.Id, user.Username, user.Password));
                return result > 0;
            }
            return false;
        }
    }
}
