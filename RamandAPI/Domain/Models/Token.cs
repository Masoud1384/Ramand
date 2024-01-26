namespace Domain.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string JwtToken { get; set; }
        public DateTime Expire { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExp { get; set; }
        public User user { get; set; }

        public Token(int userId,string jwtToken, DateTime expire, string refreshToken, DateTime refreshTokenExp) : this(jwtToken, expire, refreshToken, refreshTokenExp)
        {
            Id = userId;
        }

        public Token(string jwtToken, DateTime expire, string refreshToken, DateTime refreshTokenExp)
        {
            JwtToken = jwtToken;
            Expire = expire;
            RefreshToken = refreshToken;
            RefreshTokenExp = refreshTokenExp;
        }
        public Token()
        {
            
        }
    }
}
