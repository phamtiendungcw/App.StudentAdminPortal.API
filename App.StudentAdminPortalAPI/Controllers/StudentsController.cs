using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.StudentAdminPortalAPI.DomainModels;
using App.StudentAdminPortalAPI.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace App.StudentAdminPortalAPI.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            // Get students
            var students = await studentRepository.GetStudentsAsync();

            return Ok(mapper.Map<List<Student>>(students));
        }

        [HttpGet]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            // Fetch student details
            var student = await studentRepository.GetStudentAsync(studentId);

            // Return Student
            if (student == null)
                return NotFound();

            return Ok(mapper.Map<Student>(student));
        }

        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            // Update Details
            if (await studentRepository.Exits(studentId))
            {
                var updateStudent = await studentRepository.UpdateStudent(studentId, mapper.Map<DataModels.Student>(request));
                if (updateStudent != null)
                    return Ok(mapper.Map<Student>(updateStudent));
            }

            return NotFound();
        }
    }
}
