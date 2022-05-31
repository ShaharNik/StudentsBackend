using AutoMapper;
using StudentsBackend.Dto;
using StudentsRegistrations.Models;

namespace StudentsBackend.Profiles
{
    public class StudentDtoProfile : Profile
    {
        public StudentDtoProfile()
        {
            // map student model to Dto
            CreateMap<Student, StudentDto>();
            CreateMap<StudentDto, Student>();
        }
    }
}
