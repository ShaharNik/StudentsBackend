using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StudentsRegistrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsRegistrations.DB
{
    public class DbClient : IDbClient
    {
        private readonly IMongoCollection<Student> _students;
        public DbClient(IOptions<StudentStoreDbConfig> StudentStoreDbConfig)
        {
            var client = new MongoClient(StudentStoreDbConfig.Value.ConnectionString);
            var database = client.GetDatabase(StudentStoreDbConfig.Value.DatabaseName); 
            _students = database.GetCollection<Student>(StudentStoreDbConfig.Value.StudentsCollectionName);
        }
        public IMongoCollection<Student> GetStudentsCollection()
        {
            return _students;
        }
    }
}
