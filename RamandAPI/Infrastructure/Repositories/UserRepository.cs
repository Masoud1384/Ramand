using Dapper;
using Domain.IRepositories;
using Domain.Models;
using System.Data;
using System.Data.SqlClient;

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

        public int Delete(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", userId, DbType.Int32);

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

                    return connection.QueryFirstOrDefault<User>("SelectUser", parameters, commandType: CommandType.StoredProcedure);
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

                    return connection.QueryFirstOrDefault<User>("SelectUser", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

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
}
