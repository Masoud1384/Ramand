using Dapper;
using Domain.IRepositories;
using Domain.Models;
using Serilog;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString = "Server=.;Database=Ramand;User Id=sa;Password=@Admin22;Encrypt=False;";
        
        public List<Employee> GetAll()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var result = conn.Query<Employee>("SelectAllEmployees",commandType:CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return new List<Employee>();
        }

        public List<Employee> GetEmployeeByCode(int code)
        {
            try
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@code", code, DbType.Int32);
                    var userToken = connection.Query<Employee>("GetEmployeeByCode", parameters, commandType: CommandType.StoredProcedure);
                    return userToken.ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return new List<Employee>();
        }

        public bool Upsert(Employee employee)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@code", employee.Code, DbType.Int32);
                    parameters.Add("@month", employee.Month, DbType.StringFixedLength);
                    parameters.Add("@salary", employee.Salary, DbType.Int32);

                    var result = connection.Execute("UpsertEmployeeSalary", parameters, commandType: CommandType.StoredProcedure);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return false;
        }

        public bool UpsertMultiple(List<Employee> employees)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var json = JsonConvert.SerializeObject(employees);
                    var result = connection.Execute("UpsertMultipleEmployees", new { json = json }, commandType: CommandType.StoredProcedure);
                    return result > 0;
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
