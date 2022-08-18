using System.Collections.Generic;
using System.Threading.Tasks;
using App.StudentAdminPortalAPI.DataModels;

namespace App.StudentAdminPortalAPI.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();
    }
}
