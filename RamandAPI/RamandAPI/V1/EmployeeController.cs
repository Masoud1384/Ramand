using Application.EmployeesOperation.IEmployeeRepositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RamandAPI.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepositoriesApplication _employeeRepositoriesApplication;

        public EmployeeController(IEmployeeRepositoriesApplication employeeRepositoriesApplication)
        {
            _employeeRepositoriesApplication = employeeRepositoriesApplication;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var result = _employeeRepositoriesApplication.GetEmployees();
            if (result.Count > 0)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("{code}")]
        public IActionResult GetBy(int code)
        {
            var result = _employeeRepositoriesApplication.GetEmployee(code);
            if (result.Count > 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost]
        public IActionResult Post(List<Employee> employees)
        {
            var result = _employeeRepositoriesApplication.UpsertMultipleEmployees(employees);
            if (result)
            {
                return Ok("Users upserted ok");
            }
            return BadRequest(result);
        }
    }
}
