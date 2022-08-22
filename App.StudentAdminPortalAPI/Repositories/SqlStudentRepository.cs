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
    }
}
