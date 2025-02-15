﻿using Application.TokenOperations.TokenCommands;
using Application.UserOperations.Commands;
using Application.UserOperations.IRepositoryApplication;
using Domain.IRepositories;
using Domain.Models;
using Mapster;
using Serilog;

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
            try
            {
                if (!(string.IsNullOrWhiteSpace(createUserCommand.Username) && string.IsNullOrWhiteSpace(createUserCommand.Password)))
                {
                    var user = new User(createUserCommand.Username, createUserCommand.Password, new Token());
                    var result = _userRepository.Create(user);
                    if (result > 0)
                    {
                        var uservmdata = _userRepository.GetUserBy(createUserCommand.Username);
                        return new UserVM(uservmdata.Id, uservmdata.Password, uservmdata.Username);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return null;
        }

        public bool Delete(string username)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(username))
                {
                    var result = _userRepository.Delete(username);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return false;
        }

        public IEnumerable<UserVM> GetAll()
        {
            try
            {
                var result = _userRepository.GetAll();
                return result
                    .Select(user => new UserVM(user.Id, user.Username, user.Password)).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return null;
        }

        public UserVM GetUserBy(int id)
        {
            try
            {
                if (id > 0)
                {
                    var user = _userRepository.GetUserBy(id);
                    if (user != null)
                    {
                        var token = new TokenCommand(user.Id, user.Token.JwtToken, user.Token.Expire, user.Token.RefreshToken, user.Token.RefreshTokenExp);

                        return new UserVM(user.Id, user.Username, user.Password, token);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return null;
        }

        public UserVM GetUserBy(string username)
        {
            try
            {
                var user = _userRepository.GetUserBy(username);
                if (user != null)
                {
                    var token = new TokenCommand();
                    token.Adapt(user);
                    return new UserVM(user.Id, user.Username, user.Password, token);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return null;
        }

        public int InsertUsers(List<CreateUserCommand> users)
        {
            var userList = users.Adapt<List<User>>();

            if (userList.Count > 0)
            {
                return _userRepository.InsertUsers(userList);
            }
            return -1;
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
            try
            {
                var user = _userRepository.GetUserBy(updateUserCommand.Username);
                if (user.Id > 0)
                {
                    var updateUser = new User(user.Id,updateUserCommand.newUsername,updateUserCommand.newPassword);
                    return _userRepository.Update(updateUser) != 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return false;
        }
    }
}
