using Application.TokenOperations.TokenCommands;

namespace Application.UserOperations.Commands
{
    public class UpdateUserCommand : CreateUserCommand
    {
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
