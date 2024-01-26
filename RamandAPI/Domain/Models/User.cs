namespace Domain.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        // In real world projects we must store the hashed passwrod
        // but in order to avoid complexity we just store the password itself here.
    }
}
