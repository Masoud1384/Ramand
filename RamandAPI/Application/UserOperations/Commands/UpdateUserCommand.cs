namespace Application.UserOperations.Commands
{
    public class UpdateUserCommand : UserVM
    {
        public UpdateUserCommand(int id, string username, string password)
            : base(id, username, password)
        {
        }
    }
}
