
using Application.TokenOperations.TokenCommands;
using Domain.Models;

namespace Application.UserOperations.Commands
{
    public class UserVM : Domain.Models.User
    {
        public UserVM(int id,string username,string password,TokenCommand token) 
            :base(id,password,username,new Token(token.JwtToken,token.Expire,token.RefreshToken,token.RefreshTokenExp))
        {

        }

        public UserVM()
        {
                
        }

        public UserVM(int id, string password, string username) : base(id, username, password)
        {
        }
    }
}
