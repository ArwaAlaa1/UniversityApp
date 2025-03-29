
using AutoMapper;
using Lab_1.Dtos;
using Lab_1.Extentions;
using Lab_1.Interfaces;
using Lab_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Lab_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IWebHostEnvironment _environment;
        private readonly string _imagepath;
        public StudentController(IStudentRepository studentRepository,IMapper mapper,IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
           _studentRepository = studentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
           
            _environment = environment;
            _imagepath = $"{_environment.WebRootPath}/images/students";
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students =await _studentRepository.GetAllStudentsWithDept();
            if (students.Count() == 0)
            {
                return NotFound();
            }
            var studentDto = _mapper.Map<IEnumerable<StudentDto>>(students);
            return Ok(studentDto);
        }

        [HttpGet("{id}")]
        [Authorize(Roles ="Admin,Student")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student =await  _studentRepository.GetStudentWithDept(id);
            if (student == null)
            {
                return NotFound();
            }
            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }


        [HttpGet("ByName/{name}")]
        public async Task<IActionResult> GetStudentByName(string name)
        {
            var students = await _studentRepository.GetStudentsWithName(name);
            if (students == null)
            {
                return NotFound();
            }
            var studentDto = _mapper.Map<IEnumerable<StudentDto>>(students);
            return Ok(studentDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromForm] StudentSendDto student)
        {

            var imagepath= ImagesHelper.GetPathImage(student.Image.FileName);

            using var stream = System.IO.File.Create(imagepath);
            student.Image.CopyTo(stream);

            var studentAdded = _mapper.Map<Student>(student);
            studentAdded.Image = imagepath.Split(Path.DirectorySeparatorChar).ElementAt(6);
            studentAdded.Age = DateTime.Now.Year - student.DOB.Year;
            _unitOfWork.Repository<Student>().Add(studentAdded);
            await _unitOfWork.CompleteAsync();
           
            return CreatedAtAction(nameof(GetStudentById), new { id = studentAdded.Id }, new { message = "created" });

        }
        [HttpPut]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> UpdateStudent(StudentSendDto studentSend)
        {
 
            var student = await _unitOfWork.Repository<Student>().GetById(studentSend.Id);
            if (student == null)
            {
                return NotFound(new { message = "Student not found." });
            }

            
            ImagesHelper.Delete(student.Image);

            
            var imagepath = ImagesHelper.GetPathImage(studentSend.Image.FileName);
            using var stream = System.IO.File.Create(imagepath);
            studentSend.Image.CopyTo(stream);

            
            _mapper.Map(studentSend, student);
            student.Image = imagepath.Split(Path.DirectorySeparatorChar).Last(); 

            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Updated Successfully" });
        }


        [HttpDelete]
        public async  Task<IActionResult> Delete(int id)
        {
            var studentDeleted= await _unitOfWork.Repository<Student>().GetById(id);
            studentDeleted.IsDeleted = true;
           _unitOfWork.Repository<Student>().Edit(studentDeleted);
            await _unitOfWork.CompleteAsync();
            return Ok(new {message =$"Student {studentDeleted.Name} is Deleted Sucessfully"});
        }

        [HttpPatch("{id}/{name}")]
        public async Task<IActionResult> UpdateName(int id, [FromRoute] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { message = "Name cannot be empty." });
            }

            var student = await _unitOfWork.Repository<Student>().GetById(id);
            if (student == null)
            {
                return NotFound(new { message = $"Student with id {id} not found." });
            }

            student.Name = name;
            _unitOfWork.Repository<Student>().Edit(student);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = $"Student name with id {student.Id} updated successfully." });
        }

    }
}
