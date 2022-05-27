using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentsRegistrations.Models;

namespace StudentsBackend.Models
{
    public class StudentCSVResult : FileResult
    {
        private readonly IEnumerable<Student> _StudentData;
        public StudentCSVResult(IEnumerable<Student> StudentData, string fileDownloadName) : base("text/csv")
        {
            _StudentData = StudentData;
            FileDownloadName = fileDownloadName;
        }
        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });
            using (var streamWriter = new StreamWriter(response.Body))
            {
                await streamWriter.WriteLineAsync(
                  $"Id, FirstName, LastName"
                );
                foreach (var s in _StudentData)
                {
                    await streamWriter.WriteLineAsync(
                      $"{s.studentId}, {s.firstName}, {s.lastName}"
                    );
                    await streamWriter.FlushAsync();
                }
                await streamWriter.FlushAsync();
            }
        }
    }
}
