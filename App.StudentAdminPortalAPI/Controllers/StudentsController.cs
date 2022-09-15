using App.StudentAdminPortalAPI.DomainModels;
using App.StudentAdminPortalAPI.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace App.StudentAdminPortalAPI.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper, IImageRepository imageRepository)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        [HttpGet]
        [Route("[controller]/GetAll")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            // Get students
            var students = await _studentRepository.GetStudentsAsync();
            return Ok(_mapper.Map<List<Student>>(students));
        }

        [HttpGet]
        [Route("[controller]/GetDetail/{studentId:guid}"), ActionName("GetStudentAsync")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            // Fetch student details
            var student = await _studentRepository.GetStudentAsync(studentId);

            // Return Student
            if (student == null)
                return NotFound();
            return Ok(_mapper.Map<Student>(student));
        }

        [HttpPut]
        [Route("[controller]/Update/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            // Update Details
            if (await _studentRepository.Exits(studentId))
            {
                var updateStudent = await _studentRepository.UpdateStudent(studentId, _mapper.Map<DataModels.Student>(request));
                if (updateStudent != null)
                    return Ok(_mapper.Map<Student>(updateStudent));
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("[controller]/Delete/{studentId:guid}")]
        public async Task<ActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await _studentRepository.Exits(studentId))
            {
                // Delete th student
                var student = await _studentRepository.DeleteStudent(studentId);
                return Ok(_mapper.Map<Student>(student));
            }
            return NotFound();
        }

        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {
            var student = await _studentRepository.AddStudent(_mapper.Map<DataModels.Student>(request));
            return CreatedAtAction(nameof(GetStudentAsync), new { studentId = student.Id }, _mapper.Map<Student>(student));
        }

        [HttpPost]
        [Route("[controller]/{studentId:guid}/Upload-Image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            var validExtensions = new List<string>()
            {
                "jpeg",
                "png",
                "gif",
                "jpg"
            };

            // Check count images
            if (profileImage != null && profileImage.Length > 0)
            {
                var extension = Path.GetExtension(profileImage.FileName);
                if (validExtensions.Contains(extension))
                {
                    // Check if student exists
                    if (await _studentRepository.Exits(studentId))
                    {
                        // Upload the Image to local storage
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
                        var fileImagePath = await _imageRepository.Upload(profileImage, fileName);

                        // Update the profile image path in database
                        if (await _studentRepository.UpdateProfileImage(studentId, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }
                        return StatusCode(StatusCodes.Status500InternalServerError, "Xảy ra sự cố khi tải ảnh lên!");
                    }
                }

                return BadRequest("Không phải là một giá trị định dạng hình ảnh");
            }

            return NotFound();
        }
    }
}