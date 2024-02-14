
using Domain.IRepositories;
using Domain.Models;

namespace Application.EmployeesOperation.EmployeeRepositories
{
    public class EmployeeRepositoriesApplication : IEmployeeRepositories.IEmployeeRepositoriesApplication
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeRepositoriesApplication(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }


        public List<Employee> GetEmployee(int code)
        {
            return _employeeRepository.GetEmployeeByCode(code);
        }

        public List<Employee> GetEmployees()
        {
            var result = _employeeRepository.GetAll();
            if (result.Count > 0)
            {
                return result;
            }
            return new List<Employee>();
        }

        public bool UpsertEmployee(Employee employee)
        {
            if (employee.Code> 0 )
            {
                return _employeeRepository.Upsert(employee);
            }
            return false;
        }

        public bool UpsertMultipleEmployees(List<Employee> employees)
        {
            if (employees.Count > 0)
            {
                return _employeeRepository.UpsertMultiple(employees);
            }
            return false;
        }
    }
}
