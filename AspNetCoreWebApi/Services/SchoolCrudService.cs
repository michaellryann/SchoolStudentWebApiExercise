using AspNetCoreWebApi.Data;
using AspNetCoreWebApi.Models;
using AspNetCoreWebApi.Sql.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApi.Services
{
    /// <summary>
    /// Service class for providing School's CRUD transactions.
    /// </summary>
    public class SchoolCrudService
    {
        private readonly TurboBootcampDbContext _db;

        public SchoolCrudService(TurboBootcampDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get all school data.
        /// </summary>
        /// <returns></returns>
        public async Task<List<SchoolViewModel>> GetAsync(string? schoolName)
        {
            List<SchoolViewModel> schools = null;

            if (!string.IsNullOrEmpty(schoolName))
            {
                // Uppercase the search string to filter case-insensitive.
                // Example: searchString = app -> APP.
                var searchString = schoolName.ToUpper();

                 schools = await _db.Schools
                    .AsNoTracking()
                    .Where(Q => Q.Name.ToUpper().StartsWith(searchString))
                    .Select(Q => new SchoolViewModel
                    {

                        SchoolId = Q.SchoolId,
                        SchoolName = Q.Name,
                        EstablishedAt = Q.EstablishedAt

                    })
                    .ToListAsync();
            }
            else
            {
                schools = await _db.Schools
               // Use AsNoTracking() for read-only / SELECT queries to improve the performance.
               .AsNoTracking()
               .Select(Q => new SchoolViewModel
               {

                   SchoolId = Q.SchoolId,
                   SchoolName = Q.Name,
                   EstablishedAt = Q.EstablishedAt

               })
               .ToListAsync();
            }
            
            return schools;
        }

        /// <summary>
        /// Create a new school data from user's input.
        /// </summary>
        /// <param name="newSchool"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(CreateSchoolFormModel newSchool)
        {
            // Create the School entity object.
            var school = new School
            {
                Name = newSchool.Name!,
                EstablishedAt = DateTime.UtcNow
            };

            // This will INSERT the new data.
            _db.Schools.Add(school);

            // You must invoke this method, otherwise your data changes (INSERT) will not be executed.
            // You must always invoke this method at the end, and only call this once even if you have multiple INSERT, UPDATE, or DELETE at the same process,
            // unless you need really need to invoke this twice or more because of some conditions (not recommended, should just refactor your codes).
            await _db.SaveChangesAsync();

            // Because school_id column is an IDENTITY / auto-generated value, after you have invoked the SaveChangesAsync() method,
            // it will automatically append the INSERTED ID value, because your school object is still tracked.
            return school.SchoolId;
        }

        public async Task<bool> UpdateAsync(int schoolId, UpdateSchoolRequestModel updatedSchool)
        {
            var existingSchool = _db.Schools
                .FirstOrDefault(Q => Q.SchoolId == schoolId);

            if (existingSchool == null)
            {
                return false;
            }

            existingSchool.Name = updatedSchool.SchoolName!;
           
            await _db.SaveChangesAsync();

            return true;
        }

        
    }
}
