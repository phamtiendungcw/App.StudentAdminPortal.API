using App.StudentAdminPortalAPI.DataModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace App.StudentAdminPortalAPI.Repositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly StudentAdminContext context;

        public SqlStudentRepository(StudentAdminContext context)
        {
            this.context = context;
        }

        public async Task<bool> Exits(Guid studentId)
        {
            return await context.Student.AnyAsync(x => x.Id == studentId);
        }

        public async Task<List<Gender>> GetGendersAsync()
        {
            return await context.Gender.ToListAsync();
        }

        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            return await context.Student
                                .Include(nameof(Gender))
                                .Include(nameof(Address))
                                .FirstOrDefaultAsync(x => x.Id == studentId);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await context.Student
                                .Include(nameof(Gender))
                                .Include(nameof(Address))
                                .ToListAsync();
        }

        public async Task<Student> UpdateStudent(Guid studentId, Student request)
        {
            var exitingStudent = await GetStudentAsync(studentId);
            if (exitingStudent != null)
            {
                exitingStudent.FirstName = request.FirstName;
                exitingStudent.LastName = request.LastName;
                exitingStudent.DateOfBirth = request.DateOfBirth;
                exitingStudent.Email = request.Email;
                exitingStudent.Mobile = request.Mobile;
                exitingStudent.GenderId = request.GenderId;
                exitingStudent.Address.PhysicalAddress = request.Address.PhysicalAddress;
                exitingStudent.Address.PostalAddress = request.Address.PostalAddress;

                await context.SaveChangesAsync();
                return exitingStudent;
            }

            return null;
        }
    }
}
