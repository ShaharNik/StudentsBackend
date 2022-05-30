using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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

        public StudentsController(IStudentServices studentServices)
        {
            _studentsServices = studentServices;
        }
        [EnableCors("MyPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var allStudents = await _studentsServices.GetStudents();
            var studentsDtoList = new List<StudentDto>();

            if (allStudents is not null)
            {
                studentsDtoList = allStudents.Select(s => new StudentDto(s.StudentId, s.FirstName, s.LastName, s.Nation)).ToList();
            }
            return studentsDtoList.Any() ? Ok(studentsDtoList) : NotFound();
        }
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudent(string studentId)
        {
            var student = await _studentsServices.GetStudent(studentId);

            if (student is null)
                return NotFound();

            return Ok(student);
        }
        [Route("/unSubmitted")]
        [HttpGet]
        public async Task<IActionResult> GetUnsubmittedStudents()
        {
            var unsubmittedStudents = await _studentsServices.GetUnsubmittedStudents();
            return Ok(unsubmittedStudents);
        }
        [Route("/api/Student")]
        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] Student newStudent)
        {
            if (!Validation.isValidStudents(newStudent))
                return BadRequest("unvalid student");
            if (!ModelState.IsValid)
                return BadRequest("unvalid student");

            var studentCreated = await _studentsServices.InsertStudent(newStudent); // change to DTO, (not expose isSubmitted to client)

            if (studentCreated == null)
                return BadRequest("Student ID already exist");
            
            //return CreatedAtRoute("Students", new { id = newStudent.id }, newStudent);
            return CreatedAtAction(nameof(AddStudent), new { id = newStudent.id }, newStudent);
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

            updatedStudent.id = student.id;

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