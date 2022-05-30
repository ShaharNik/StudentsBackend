using StudentsRegistrations.Models;

namespace StudentsBackend.Models
{
    public class Validation
    {
        public static bool isValidStudents(Student student)
        {
            if (student == null)
                return false;
            else if (student.StudentId == null)
                return false;
            else if (student.StudentId.Length != 9)
                return false;
            else if (student.Nation.Length < 3)
                return false;
            else if (student.LastName.Length < 2)
                return false;
            else if (student.FirstName.Length < 2)
                return false;

            return true;
        }
    }
}
