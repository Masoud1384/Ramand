using Application.TokenOperations.TokenCommands;
using System.Text.Json.Serialization;

namespace Application.UserOperations.Commands
{
    public class CreateUserCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public TokenCommand? Token { get; set; }

        public CreateUserCommand()
        {

        }
        public CreateUserCommand(string username,string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
