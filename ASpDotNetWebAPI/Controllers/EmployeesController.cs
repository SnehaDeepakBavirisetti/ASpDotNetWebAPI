using ASpDotNetWebAPI.Data;
using ASpDotNetWebAPI.Models;
using ASpDotNetWebAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ASpDotNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbcontext applicationDbcontext;

        public EmployeesController(ApplicationDbcontext applicationDbcontext)
        {
            this.applicationDbcontext = applicationDbcontext;
        }
        //[httpget]
        //public iactionresult getemployees()
        //{
        //    var allemployees = applicationdbcontext.employees.tolist();
        //    return ok(allemployees);
        //}
        [HttpGet]
        [Authorize]
        public IActionResult GetEmployeeSpecific(Guid id)
        {
            var employee = applicationDbcontext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            EmployeeDTO employeedto = new EmployeeDTO()
            {
                Age = employee?.Age,
                Name = employee?.Name,
                Phone = employee?.Phone,
                Email = employee?.Email
            };

            return Ok(employeedto);
        }
        [HttpPost]
        [Authorize]
        public IActionResult PutEmployee(EmployeeDTO employeedto)
        {
            var employee = new Employee()
            {
                Name = employeedto.Name,
                Age = employeedto.Age,
                Email = employeedto.Email,
                Phone = employeedto.Phone
            };

            applicationDbcontext.Employees.Add(employee);
            applicationDbcontext.SaveChanges();
            return Ok(employee);
        }

        [HttpPut]
        [Authorize]
        public IActionResult updateEmplyeeDTO(Guid id, UpdateEmplyeeDTO updateEmplyeeDTO)
        {
            var employee = applicationDbcontext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }
            employee.Name = updateEmplyeeDTO.Name;
            employee.Age = updateEmplyeeDTO.Age;
            employee.Email = updateEmplyeeDTO.Email;
            employee.Phone = updateEmplyeeDTO.Phone;
            employee.Salary = updateEmplyeeDTO.Salary;

            applicationDbcontext.SaveChanges();

            return Ok(employee);

        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteEmplyeeDTO(Guid id)
        {
            var employee = applicationDbcontext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }
            applicationDbcontext.Employees.Remove(employee);
            applicationDbcontext.SaveChanges();

            return Ok(employee);
        }
    }
}
