using Dapper;
using Domain.IRepositories;
using Domain.Models;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString = "Server=.;DataBase=Ramand;Trusted_Connection=True;Encrypt=False;";

        public int Create(User user)
        {
            // using try-catch isn't a proper way to handle errors however since we're building a test
            // project we use it but in the big and serious projects we must design a way to handle errors.
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Username", user.Username, DbType.String);
                    parameters.Add("@Password", user.Password, DbType.String);

                    return connection.Execute("InsertUser", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public int Delete(string username)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@username", username, DbType.String);

                    return connection.Execute("DeleteUser", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    return connection.Query<User>("SelectAllUsers",
                        commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public User GetUserBy(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", id, DbType.Int32);

                    var userToken = connection.QueryFirstOrDefault<UserToken>("SelectUserById", parameters, commandType: CommandType.StoredProcedure);

                    if (userToken != null)
                    {
                        var user = new User(userToken.Id, userToken.Username, userToken.Password);
                        var token = new Token(userToken.Id, userToken.Token, userToken.Expire, userToken.RefreshToken, userToken.RefreshTokenExp);
                        user.Token = token;
                        return user;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public User GetUserBy(string username)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Username", username, DbType.String);

                    var userToken = connection.QueryFirstOrDefault<UserToken>("SelectUserByUsername", parameters, commandType: CommandType.StoredProcedure);

                    if (userToken != null)
                    {
                        var user = new User(userToken.Id, userToken.Username, userToken.Password);
                        var token = new Token(userToken.Id, userToken.Token, userToken.Expire, userToken.RefreshToken, userToken.RefreshTokenExp);
                        user.Token = token;
                        return user;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public bool IsUsernameExist(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<int>("EXEC Ramand.dbo.IsUsernameExist @username", new { username }).SingleOrDefault();
                return result == 1;
            }
        }
        #region DRY 
        // this is another way to build the GetUser method without repeating the code
        // but it has a slight impact on performance so i'd prefer to use the upper way



        //public User GetUserBy(dynamic value)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            connection.Open();

        //            var parameters = new DynamicParameters();
        //            string parameterName;
        //            DbType dbType;

        //            if (value is int)
        //            {
        //                parameterName = "@Id";
        //                dbType = DbType.Int32;
        //            }
        //            else if (value is string)
        //            {
        //                parameterName = "@Username";
        //                dbType = DbType.String;
        //            }
        //            else
        //            {
        //                throw new ArgumentException("Value must be an integer or a string.");
        //            }

        //            parameters.Add(parameterName, value, dbType);

        //            return connection.QueryFirstOrDefault<User>("SelectUser", parameters, commandType: CommandType.StoredProcedure);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //}
        #endregion

        public int Update(User user)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", user.Id, DbType.Int32);
                    parameters.Add("@Username", user.Username, DbType.String);
                    parameters.Add("@Password", user.Password, DbType.String);

                    return connection.Execute("UpdateUser", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
    class UserToken
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime Expire { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExp { get; set; }
    }
}
