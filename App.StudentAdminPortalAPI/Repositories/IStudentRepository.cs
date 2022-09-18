using App.StudentAdminPortalAPI.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.StudentAdminPortalAPI.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> AddStudent(Student request);

        Task<Student> DeleteStudent(Guid studentId);

        Task<bool> Exits(Guid studentId);

        Task<List<Gender>> GetGendersAsync();

        Task<Student> GetStudentAsync(Guid studentId);

        Task<List<Student>> GetStudentsAsync();

        Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl);

        Task<Student> UpdateStudent(Guid studentId, Student request);
    }
}