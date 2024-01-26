namespace Application.UserOperations.Commands
{
    public class LoginCommand
    {
        public string username { get; set; }
        public string password { get; set; }

        public LoginCommand(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public LoginCommand()
        {
                
        }
    }
}
