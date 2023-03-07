using AspNetCoreWebApi.Data;
using AspNetCoreWebApi.Models;
using AspNetCoreWebApi.Sql.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApi.Services
{
    public class StudentCrudService
    {
        private readonly TurboBootcampDbContext _dbStudent;

        public StudentCrudService(TurboBootcampDbContext dbStudent)
        {
            _dbStudent = dbStudent;
        }

        public async Task<List<StudentViewModel>> GetAsync(int page)
        {
            

            var students = await _dbStudent.Students
           // Use AsNoTracking() for read-only / SELECT queries to improve the performance.
           .AsNoTracking()
           .Select(Q => new StudentViewModel
           {

               StudentId = Q.StudentId,
               PhoneNumber = Q.PhoneNumber,
               FullName = Q.FullName,
               NickName = Q.Nickname == null ? "NONE" : Q.Nickname,
               JoinedAt = Q.JoinedAt,
               SchoolId = (int)(Q.SchoolId == null ? 0 : Q.SchoolId)

           })
           .ToListAsync();

            var itemPerPage = 5;

            if (page >= 1)
            {
                page -= 1;
            }

            students = students
                // Skip based on page X itemPerPage, example: 1 page X 3 itemPerPage, so you will skip 3 items and retrieve the next 3 items on the next page.
                .Skip(page * itemPerPage)
                // Take the whole items on the next page, example: 3 itemPerPage, then users will retrieve 3 itemPerPage
                .Take(itemPerPage)
                .ToList();


            return students;
        }
    }
}
