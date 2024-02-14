using Domain.Models;

namespace Domain.IRepositories
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetAll();
        public bool Upsert(Employee employee);
        public List<Employee> GetEmployeeByCode(int code);
        public bool UpsertMultiple(List<Employee> employees);
        //public bool DeleteEmployeeBy(int code);
    }
}
