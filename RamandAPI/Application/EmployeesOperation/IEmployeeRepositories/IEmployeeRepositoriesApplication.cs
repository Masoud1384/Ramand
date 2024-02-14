using Domain.Models;

namespace Application.EmployeesOperation.IEmployeeRepositories
{
    public interface IEmployeeRepositoriesApplication
    {
        public bool UpsertMultipleEmployees(List<Employee> employees);
       
        public bool UpsertEmployee(Employee employee);
        public List<Employee> GetEmployee(int code);

        public List<Employee> GetEmployees();
    }
}