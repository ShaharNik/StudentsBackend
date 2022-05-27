using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsRegistrations.DB
{
    public class StudentStoreDbConfig
    {
        public string? DatabaseName { get; set; }
        public string? StudentsCollectionName { get; set; }
        public string? ConnectionString { get; set; }
    }
}
