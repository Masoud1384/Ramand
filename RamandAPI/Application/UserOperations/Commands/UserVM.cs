
namespace Application.UserOperations.Commands
{
    public class UserVM : Domain.Models.User
    {
        public UserVM(int id,string username,string password) 
            :base(id,password,username)
        {

        }
        public UserVM()
        {
                
        }
    }
}
