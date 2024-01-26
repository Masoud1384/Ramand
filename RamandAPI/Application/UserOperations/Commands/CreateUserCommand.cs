namespace Application.UserOperations.Commands
{
    public class CreateUserCommand
    {
        public string username { get; set; }
        public string password { get; set; }

        public CreateUserCommand(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
