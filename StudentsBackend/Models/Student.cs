using Microsoft.Extensions.Primitives;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsRegistrations.Models
{
    public class Student
    {
        public Student(string studentId, string lastName, string firstName, string nation)
        {
            this.StudentId = studentId;
            this.LastName = lastName;
            this.FirstName = firstName;
            this.Nation = nation;
            this.IsSubmitted = false;
        }

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Student ID is required")]
        [BsonElement("studentId")]
        public string StudentId { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("nation")]
        public string Nation { get; set; }

        [BsonElement("isSubmitted")]
        public bool IsSubmitted { get; set; } // needs to be private/Dto 

        //public string Gender { get; set; }
        //public string HomePhoneNumber { get; set; }
        //public string MobilePhoneNumber { get; set; }
        //public string Email { get; set; }
        //public DateTime BirthDay { get; set; }
    }
}
