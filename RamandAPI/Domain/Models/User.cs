namespace Domain.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public User(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        // In real world projects we must store the hashed passwrod
        // but in order to avoid complexity we just store the password itself here.

    }
}
