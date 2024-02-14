using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Employee
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Month { get; set; }
        public int Salary { get; set; }
        public int Code { get; set; }
    }
}
