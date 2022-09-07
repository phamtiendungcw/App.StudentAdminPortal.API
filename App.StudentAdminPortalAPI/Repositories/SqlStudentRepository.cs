using App.StudentAdminPortalAPI.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.StudentAdminPortalAPI.Repositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly StudentAdminContext _context;

        public SqlStudentRepository(StudentAdminContext context)
        {
            _context = context;
        }

        public async Task<Student> AddStudent(Student request)
        {
            var student = await _context.Student.AddAsync(request);
            await _context.SaveChangesAsync();
            return student.Entity;
        }

        public async Task<Student> DeleteStudent(Guid studentId)
        {
            var student = await GetStudentAsync(studentId);

            if (student != null)
            {
                _context.Student.Remove(student);
                await _context.SaveChangesAsync();
                return student;
            }

            return null;
        }

        public async Task<bool> Exits(Guid studentId)
        {
            return await _context.Student.AnyAsync(x => x.Id == studentId);
        }

        public async Task<List<Gender>> GetGendersAsync()
        {
            return await _context.Gender.ToListAsync();
        }

        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            return await _context.Student
                                .Include(nameof(Gender))
                                .Include(nameof(Address))
                                .FirstOrDefaultAsync(x => x.Id == studentId);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Student
                                .Include(nameof(Gender))
                                .Include(nameof(Address))
                                .ToListAsync();
        }

        public async Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl)
        {
            var student = await GetStudentAsync(studentId);
            if (student != null)
            {
                student.ProfileImageUrl = profileImageUrl;
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
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

                await _context.SaveChangesAsync();
                return exitingStudent;
            }

            return null;
        }
    }
}