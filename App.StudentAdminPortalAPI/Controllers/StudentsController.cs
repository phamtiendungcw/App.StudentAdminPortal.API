﻿using System.Collections.Generic;
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
            var students = await studentRepository.GetStudentsAsync();

            return Ok(mapper.Map<List<Student>>(students));
        }
    }
}
