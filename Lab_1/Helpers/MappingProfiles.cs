using AutoMapper;
using Humanizer;
using Lab_1.Dtos;
using Lab_1.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Lab_1.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Department, DepartmentDto>().ForMember(d=>d.StudentNames,s=>s.MapFrom(sn=>sn.Students.Select(s => s.Name).ToList()))
                .ForMember(d=>d.CountOfStudents,s=>s.MapFrom(sc=>sc.Students.Count()))
                .ForMember(d=>d.Message,s=>s.MapFrom(sm=>sm.Students.Count() > 2 ? "This class is overloaded" : "This class is normal"));

            CreateMap<Department, DepartmentSendDto>().ReverseMap();

            CreateMap<Student,StudentDto>().ForMember(s => s.deptName, d => d.MapFrom(dn => dn.Dept.Name))
                .ForMember(s => s.Skills, d => d.MapFrom(sk => new List<string> { "C#", "Java", "Python" })).ReverseMap();

            CreateMap<StudentSendDto, Student>()
          .ForMember(s => s.Age, d => d.MapFrom(sa => DateTime.Now.Year - sa.DOB.Year))
          .ReverseMap();
        }
    }
}
