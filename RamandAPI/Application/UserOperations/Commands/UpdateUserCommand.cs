using Application.TokenOperations.TokenCommands;

namespace Application.UserOperations.Commands
{
    public class UpdateUserCommand : CreateUserCommand
    {
        public int id { get; set; }

        public UpdateUserCommand(int id, string username, string password,TokenCommand token)
        {
            this.id = id;
            this.username = username;
            this.password = password;
        }
        public UpdateUserCommand()
        {
                
        }
    }
}
