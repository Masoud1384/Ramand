using Domain.Models;

namespace Domain.IRepositories
{
    public interface ITokenRepository
    {
        Token CreateRefreshToken(string refreshToken);
        Token GetRefreshToken(string refreshToken);
        bool SaveToken(int userId,Token token);
    }
}
