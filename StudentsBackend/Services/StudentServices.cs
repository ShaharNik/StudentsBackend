using MongoDB.Bson;
using MongoDB.Driver;
using StudentsBackend.Dto;
using StudentsRegistrations.DB;
using StudentsRegistrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsRegistrations.Services
{
    public class StudentServices : IStudentServices
    {
        private readonly IMongoCollection<Student> _students;

        public StudentServices(IDbClient dbClient)
        {
            _students = dbClient.GetStudentsCollection();
        }
        public async Task<IEnumerable<Student>> GetStudents()
        {
            var allStudents = await _students.AsQueryable().ToListAsync();
            return allStudents;
        }

        public async Task<Student?> InsertStudent(Student newStudent)
        {
            var checkIdNotExist = await _students.Find(student => student.StudentId == newStudent.StudentId).FirstOrDefaultAsync();
            if (checkIdNotExist == null)
            {
                await _students.InsertOneAsync(newStudent);
                return newStudent;
            }
            return null;
        }

        public async Task<long> countStudentsByNation(string nation)
        {
            var nationFilter = Builders<Student>.Filter.Where(student => student.Nation.Equals(nation));
            long countResult = await _students.CountDocumentsAsync(nationFilter);
            return countResult;
        }

        public async Task DeleteStudent(string studentId)
        {
            await _students.DeleteOneAsync(student => student.Id == studentId);
        }

        public async Task<Student> GetStudent(string studentId)
        {
            var student = await _students.Find(student => student.StudentId == studentId).FirstOrDefaultAsync();
            return student;
        }


        public async Task<IEnumerable<Student>> GetUnsubmittedStudents()
        {
            var allUnsubmittedStudents = await _students.Find(student => student.IsSubmitted == false).ToListAsync();
            return allUnsubmittedStudents;
        }

        public async Task UpdateStudent(string studentId, Student updatedStudent)
        {
            await _students.ReplaceOneAsync(s => s.StudentId == updatedStudent.StudentId, updatedStudent);
        }

        public async Task SubmitAllUnsubmittedStudents()
        {
            var updateDefinition = Builders<Student>.Update.Set(s => s.IsSubmitted, true);
            await _students.UpdateManyAsync(s => s.IsSubmitted == false, updateDefinition);
        }

    }
}

