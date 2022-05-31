using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StudentsBackend.Dto;
using StudentsBackend.Models;
using StudentsRegistrations.Models;
using StudentsRegistrations.Services;
using System.Text;

namespace StudentsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentServices _studentsServices;
        private readonly IMapper _mapper;

        public StudentsController(IStudentServices studentServices, IMapper mapper)
        {
            _studentsServices = studentServices;
            _mapper = mapper;
        }
        [EnableCors("MyPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var allStudents = await _studentsServices.GetStudents();
            var allStudentsDto = _mapper.Map<IEnumerable<StudentDto>>(allStudents);

            return allStudentsDto.Any() ? Ok(allStudentsDto) : NotFound();
        }
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudent(string studentId)
        {
            var student = await _studentsServices.GetStudent(studentId);
            if (student is null)
                return NotFound();
            var studentDto = _mapper.Map<StudentDto>(student);

            return Ok(studentDto);
        }
        [Route("/unSubmitted")]
        [HttpGet]
        public async Task<IActionResult> GetUnsubmittedStudents()
        {
            var unsubmittedStudents = await _studentsServices.GetUnsubmittedStudents();
            var allunsubmittedStudentsDto = _mapper.Map<IEnumerable<StudentDto>>(unsubmittedStudents);
            return Ok(allunsubmittedStudentsDto);
        }
        [Route("/api/Student")]
        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentDto newStudentDto)
        {
            var newStudent = _mapper.Map<Student>(newStudentDto);
            if (!Validation.isValidStudents(newStudent))
                return BadRequest("unvalid student");
            if (!ModelState.IsValid)
                return BadRequest("unvalid student");

            var studentCreated = await _studentsServices.InsertStudent(newStudent); 

            if (studentCreated == null)
                return BadRequest("Student ID already exist");
            
            //return CreatedAtRoute("Students", new { id = newStudent.id }, newStudent);
            return CreatedAtAction(nameof(AddStudent), new { id = newStudent.Id }, newStudent);
        }
        [HttpDelete("{studentId}")]   
        public async Task<IActionResult> DeleteStudent(string studentId)
        {
            var student = await _studentsServices.GetStudent(studentId);

            if (student is null)
                return NotFound();

            await _studentsServices.DeleteStudent(studentId);

            return NoContent();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStudent(string studentId, Student updatedStudent)
        {
            if (!Validation.isValidStudents(updatedStudent))
                return BadRequest("unvalid student");

            var student = await _studentsServices.GetStudent(studentId);

            if (student is null)
                return NotFound();

            updatedStudent.Id = student.Id;

            await _studentsServices.UpdateStudent(studentId, updatedStudent);

            return NoContent();
        }
        [Route("/ExportToCSV")]
        [HttpGet]
        public async Task<StudentCSVResult> ExportToCSV()
        {
            var allUnsubmittedStudents = await _studentsServices.GetUnsubmittedStudents();
            var fileDownloadName = "unsubmittedStudents.csv";
            return new StudentCSVResult(allUnsubmittedStudents, fileDownloadName);
        }
        [Route("/SubmitStudents")]
        [HttpPut] 
        public async Task<IActionResult> SubmitStudents()
        {
            await _studentsServices.SubmitAllUnsubmittedStudents();

            return Ok();
        }
    }
}