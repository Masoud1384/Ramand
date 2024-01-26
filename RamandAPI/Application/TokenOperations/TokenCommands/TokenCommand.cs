using Domain.Models;

namespace Application.TokenOperations.TokenCommands
{
    public class TokenCommand : Domain.Models.Token
    {


        public TokenCommand() { }

        public TokenCommand(string jwtToken, DateTime expire, string refreshToken, DateTime refreshTokenExp) : base(jwtToken, expire, refreshToken, refreshTokenExp)
        {
        }

        public TokenCommand(int userId, string jwtToken, DateTime expire, string refreshToken, DateTime refreshTokenExp) : base(userId ,jwtToken, expire, refreshToken, refreshTokenExp)
        {
        }
    }
}
