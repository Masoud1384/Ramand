namespace Domain.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public Token Token { get; set; }

        public User(int id, string password, string username, Token token)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public User(string username, string password, Token token)
        {
            Username = username;
            Password = password;
            Token = token;
        }
        public User()
        {

        }
        public User(int userId, string username, string password)
        {
            this.Id = userId;
            this.Username = username;
            this.Password = password;
        }

        // In real world projects we must store the hashed passwrod
        // but in order to avoid complexity we just store the password itself here.

    }
}
