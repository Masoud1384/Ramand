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
            var result = _userRepository.GetAll();
            return result
                .Select(user => new UserVM(user.Id, user.Username, user.Password)).ToList();
        }


        public UserVM GetUserBy(int id)
        {
            var user = _userRepository.GetUserBy(id);
            if (user!=null)
            {
                return new UserVM(user.Id, user.Username, user.Password);
            }
            return new UserVM();
        }

        public UserVM GetUserBy(string username)
        {
            var user = _userRepository.GetUserBy(username);
            if (user != null)
            {
                return new UserVM(user.Id, user.Username, user.Password);
            }
            return new UserVM();
        }

        public bool Update(UpdateUserCommand user)
        {
            if (user.id > 0)
            {
                var result = _userRepository.Update(new User(user.id, user.password, user.username));
                return result > 0;
            }
            return false;
        }
    }
}
