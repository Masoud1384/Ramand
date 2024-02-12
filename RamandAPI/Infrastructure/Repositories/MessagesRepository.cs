using Domain.IRepositories;
using Domain.Models;
using Newtonsoft.Json;
using Serilog;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace RabbitDI.MessagesOperations
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly static string _connectionString = "Server=.;Database=Ramand;User Id=sa;Password=@Admin22;Encrypt=False;";
        public void DeleteAllMessages()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute("DeleteAllMessages", commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public bool InsertMessages(List<Messages> msgs)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var json = JsonConvert.SerializeObject(msgs);
                    var affectedrows = connection.Execute("InsertMessages", new { json }, commandType: CommandType.StoredProcedure);
                    return affectedrows > 0;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return false;
        }


        public bool InsertMessage(Messages msg)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var affectedRows = connection.Execute("CreateMessage", new { msg.message}, commandType: CommandType.StoredProcedure);
                    return affectedRows > 0;
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
