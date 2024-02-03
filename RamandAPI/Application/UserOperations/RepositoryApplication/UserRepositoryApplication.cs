using Application.TokenOperations.TokenCommands;
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

        public UserVM Create(CreateUserCommand createUserCommand)
        {
            if (!(string.IsNullOrWhiteSpace(createUserCommand.username) && string.IsNullOrWhiteSpace(createUserCommand.password)))
            {
                var user = new User(createUserCommand.username, createUserCommand.password, new Token());
                var result = _userRepository.Create(user);
                if (result > 0)
                {
                    var uservmdata = _userRepository.GetUserBy(createUserCommand.username);
                    return new UserVM(uservmdata.Id, uservmdata.Password, uservmdata.Username);
                }
            }
            return null;
        }

        public bool Delete(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                var result = _userRepository.Delete(username);
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
            if (user != null)
            {
                var token = new TokenCommand(user.Id, user.Token.JwtToken, user.Token.Expire, user.Token.RefreshToken, user.Token.RefreshTokenExp);

                return new UserVM(user.Id, user.Username, user.Password, token);
            }
            return new UserVM();
        }

        public UserVM GetUserBy(string username)
        {
            var user = _userRepository.GetUserBy(username);
            if (user != null)
            {
                var token = new TokenCommand(user.Id, user.Token.JwtToken, user.Token.Expire, user.Token.RefreshToken, user.Token.RefreshTokenExp);

                return new UserVM(user.Id, user.Username, user.Password, token);
            }
            return new UserVM();
        }

        public bool IsUsernameExist(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return _userRepository.IsUsernameExist(username);
            }
            return true;
            // if the useranme was empty or null we return true so it assumes that it's taken and becomes unavaliable
        }

        public bool Update(UpdateUserCommand updateUserCommand)
        {
            if (updateUserCommand.userId > 0)
            {
                var user = new User(updateUserCommand.userId, updateUserCommand.username, updateUserCommand.password);
                return _userRepository.Update(user) != 0;
            }
            return false;
        }
    }
}
