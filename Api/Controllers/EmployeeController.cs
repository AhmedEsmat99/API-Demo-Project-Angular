using Api.Model;
using Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        IRepoEmployees repo;

        public EmployeeController(IRepoEmployees repo)
        {
            this.repo = repo;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(repo.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Employee e = repo.GetById(id);
            if (e != null)
                return Ok(e);
            else
                return BadRequest(new { message = "Employee with the given ID not found: " + id });
        }
        [HttpGet("{email}")]
        public IActionResult GetByEmail(string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Employee e = repo.GetByEmail(email);
            if (e != null)
                return Ok(e);
            else
                return NotFound(new { message = "Employee with the specified email not found" });
        }
        [HttpPost]
        public IActionResult Insert(Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Employee existingEmployee = repo.GetByEmail(employee.Email);
            if (existingEmployee != null)
                return BadRequest(new { message = "Email already exists" });
            var insertResult = repo.Insert(employee);
            return insertResult ? Ok(new { message = "Data saved successfully" }) : BadRequest(new { message = "Failed to insert employee." });
        }
        [HttpPut]
        public IActionResult Update(Employee employee, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Employee existingEmployee = repo.GetById(id);
            if (existingEmployee == null)
                return BadRequest(new { message = "Employee ID not found" });
            Employee e = repo.GetByEmail(employee.Email);
            if (e != null && e.Id != id)
                return BadRequest(new { message = "Email already exists" });
            var updateResult = repo.Update(employee, id);
            return updateResult ? Ok(new { message = "Data saved successfully" }) : BadRequest(new { message = "Failed to update employee." });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Employee e = repo.GetById(id);
            if (e != null)
            {
                var deleteResult = repo.Delete(id);
                return deleteResult ? Ok(new { message = "Data saved successfully" }) : StatusCode(500, new { message = "Failed to delete employee." });
            }
            return BadRequest(new { message = "This ID does not exist" });
        }
    }
}
