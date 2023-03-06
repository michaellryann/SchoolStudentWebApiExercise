using AspNetCoreWebApi.Models;
using AspNetCoreWebApi.Services;
using AspNetCoreWebApi.Sql.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AspNetCoreWebApi.Controllers
{
    [Route("api/v1/school")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly SchoolCrudService _schoolCrudService;

        public SchoolController(SchoolCrudService schoolCrudService)
        {
            _schoolCrudService = schoolCrudService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string? schoolName)
        {
            var schools = await _schoolCrudService.GetAsync(schoolName);

            return Ok(schools);
        }

       

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateSchoolFormModel newSchool)
        {
            if (!ModelState.IsValid)
            {
                // Will return HTTP status code 400, which indicates user input errors due validation etc.
                return ValidationProblem(ModelState);
            }

            var newId = await _schoolCrudService.CreateAsync(newSchool);

            return Ok(newId);
        }

        [HttpPut("{schoolId}")]
        public async Task<ActionResult> Update(int schoolId, [FromBody] UpdateSchoolRequestModel updatedSchool)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var isSuccess = _schoolCrudService.UpdateAsync(schoolId, updatedSchool);

            if (!await isSuccess)
            {
                // This will add a new model state error to the current ModelState object.
                // We need to do this because we use a custom validation that is outside of data annotation validation in the model class.
                ModelState.AddModelError("SchoolID", "School ID tidak ditemukan.");
                return ValidationProblem(ModelState);
            }

            return Ok();
        }

        [Obsolete("Raw query version.")]
        [HttpGet("raw")]
        public ActionResult GetObsolete()
        {
            var schools = new List<SchoolViewModel>();
            var connString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=turbo_bootcamp;";
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var selectCommand = conn.CreateCommand())
                {
                    var query = @"
                    SELECT school_id,
                    name,
                    established_at
                    FROM schools";
                    selectCommand.CommandText = query;

                    // Use reader to SELECT data.
                    using (var reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var school = new SchoolViewModel
                            {
                                // Get the school_id, which column index in the SELECT query is 0.
                                SchoolId = reader.GetInt32(0),
                                SchoolName = reader.GetString(1),
                                EstablishedAt = reader.GetDateTime(2)
                            };

                            schools.Add(school);
                        }
                    }
                }
                
            }

            return Ok(schools);
        }
    }
}
