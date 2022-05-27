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

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var allStudents = await _studentsServices.GetStudents();
            return Ok(allStudents);
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
        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] Student newStudent)
        {
            // add validation
            if (newStudent is null)
                return BadRequest("Student is null");
            await _studentsServices.InsertStudent(newStudent);
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
            var student = await _studentsServices.GetStudent(studentId);

            if (student is null)
                return NotFound();

            updatedStudent.id = student.id;

            await _studentsServices.UpdateStudent(studentId, updatedStudent);

            return NoContent();
        }
        [Route("/ExportToCSV")]
        public async Task<IActionResult> ExportToCSV()
        {
            var allUnsubmittedStudents = await _studentsServices.GetUnsubmittedStudents();
            var fileDownloadName = "unsubmittedStudents.csv";
            return new StudentCSVResult(allUnsubmittedStudents, fileDownloadName);
        }
    }
}