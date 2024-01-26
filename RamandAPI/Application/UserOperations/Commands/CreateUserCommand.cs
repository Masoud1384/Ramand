using Application.TokenOperations.TokenCommands;
using System.Text.Json.Serialization;

namespace Application.UserOperations.Commands
{
    public class CreateUserCommand
    {
        public string username { get; set; }
        public string password { get; set; }
        [JsonIgnore]
        public TokenCommand? Token { get; set; }

        public CreateUserCommand()
        {

        }
        public CreateUserCommand(string username,string password, TokenCommand token)
        {
            this.username = username;
            this.password = password;
            Token = token;
        }
    }
}
