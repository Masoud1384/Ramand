using Dapper;
using Domain.IRepositories;
using Domain.Models;
using System.Data.SqlClient;
using System.Data;
using Serilog;

namespace Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly string _connectionString = "Server=.;Database=Ramand;User Id=sa;Password=@Admin22;Encrypt=False;";
        private readonly IUserRepository _userRepository;

        public TokenRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Token CreateRefreshToken(string refreshToken)
        {
            try
            {
                var userToken = GetRefreshToken(refreshToken);
                userToken.RefreshToken = Guid.NewGuid().ToString();
                userToken.RefreshTokenExp = DateTime.Now.AddDays(30);

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", userToken.Id, DbType.Int32);
                    parameters.Add("@RefreshToken", userToken.RefreshToken, DbType.String);
                    parameters.Add("@RefreshTokenExp", userToken.RefreshTokenExp, DbType.DateTime);

                    connection.Execute("UpdateRefreshToken", parameters, commandType: CommandType.StoredProcedure);
                }

                return userToken;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return null;
        }

        public Token GetRefreshToken(string refreshToken)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@RefreshToken", refreshToken, DbType.String);

                    return connection.QueryFirstOrDefault<Token>("FindRefreshToken", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return null;
        }

        public bool SaveToken(int userId, Token token)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(token.JwtToken))
                {
                    var user = _userRepository.GetUserBy(userId);
                    if (user != null)
                    {
                        using (var connection = new SqlConnection(_connectionString))
                        {
                            connection.Open();

                            var parameters = new DynamicParameters();
                            parameters.Add("@Id", userId, DbType.Int32);
                            parameters.Add("@Token", token.JwtToken, DbType.String);
                            parameters.Add("@Expire", token.Expire, DbType.DateTime);
                            parameters.Add("@RefreshToken", token.RefreshToken, DbType.String);
                            parameters.Add("@RefreshTokenExp", token.RefreshTokenExp, DbType.DateTime);

                            int rowsAffected = connection.Execute("UpsertUserToken", parameters, commandType: CommandType.StoredProcedure);
                            return rowsAffected > 0;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return false;
        }

        public bool ValidateToken(string jwtToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        var parameters = new DynamicParameters();
                        parameters.Add("@Token", jwtToken, DbType.String);

                        var result = connection.QueryFirstOrDefault<bool>("IsTokenExpired", parameters, commandType: CommandType.StoredProcedure);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return false;
        }
    }
}
