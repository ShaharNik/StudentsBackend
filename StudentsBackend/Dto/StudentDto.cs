namespace StudentsBackend.Dto
{
    public class StudentDto
    {
        public StudentDto(string studentId, string firstName, string lastName, string nation)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
            Nation = nation;
        }

        public string? Id { get; set; }
        public string? StudentId { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Nation { get; set; }
    }
}
