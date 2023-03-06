using AspNetCoreWebApi.Models;
using AspNetCoreWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace AspNetCoreWebApi.Controllers
{
    [Route("api/v1/student")]
    [ApiController]

    public class StudentController : ControllerBase
    {
        private readonly StudentCrudService _studentCrudService;

        public StudentController(StudentCrudService studentCrudService)
        {
            _studentCrudService = studentCrudService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] int page = 1)
        {
            var students = await _studentCrudService.GetAsync(page);

            return Ok(students);
        }


        
    }
}
