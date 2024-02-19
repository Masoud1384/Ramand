using Application.TokenOperations.TokenCommands;
using System.Text.Json.Serialization;

namespace Application.UserOperations.Commands
{
    public class UpdateUserCommand : CreateUserCommand
    {
        [JsonIgnore]
        public int userId { get; set; }
        public string newUsername { get; set; }
        public string newPassword { get; set; }

        public UpdateUserCommand(int id, string username, string password) : base(username, password)
        {
            this.userId = id;
        }
        public UpdateUserCommand()
        {
            
        }
    }
}
