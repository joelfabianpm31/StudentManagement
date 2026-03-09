using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using StudentManagement.Application.DTOs.Course;
using StudentManagement.Application.DTOs.Enrollment;
using StudentManagement.Application.DTOs.Student;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ── Student ──────────────────────────────────────────────────
            CreateMap<Student, StudentDto>()
                .ForMember(d => d.FullName,
                    opt => opt.MapFrom(s => s.FullName))
                .ForMember(d => d.EnrollmentCount,
                    opt => opt.MapFrom(s => s.Enrollments.Count));
            CreateMap<CreateStudentDto, Student>();
            CreateMap<UpdateStudentDto, Student>();

            // ── Course ───────────────────────────────────────────────────
            CreateMap<Course, CourseDto>()
                .ForMember(d => d.EnrolledCount, opt => opt.Ignore());
            CreateMap<CreateCourseDto, Course>();
            CreateMap<UpdateCourseDto, Course>();

            // ── Enrollment ───────────────────────────────────────────────
            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(d => d.StudentName,
                    opt => opt.MapFrom(s => s.Student.FullName))
                .ForMember(d => d.StudentEmail,
                    opt => opt.MapFrom(s => s.Student.Email));

            CreateMap<EnrollmentDetail, EnrollmentDetailDto>()
                .ForMember(d => d.CourseName,
                    opt => opt.MapFrom(s => s.Course.Name))
                .ForMember(d => d.CourseCode,
                    opt => opt.MapFrom(s => s.Course.Code))
                .ForMember(d => d.Credits,
                    opt => opt.MapFrom(s => s.Course.Credits));

            // ── EnrolledCourseDto (para pantalla cursos del estudiante) ──
            CreateMap<EnrollmentDetail, EnrolledCourseDto>()
                .ForMember(d => d.EnrollmentId, opt => opt.MapFrom(s => s.EnrollmentId))
                .ForMember(d => d.CourseId, opt => opt.MapFrom(s => s.Course.Id))
                .ForMember(d => d.CourseName, opt => opt.MapFrom(s => s.Course.Name))
                .ForMember(d => d.CourseCode, opt => opt.MapFrom(s => s.Course.Code))
                .ForMember(d => d.Credits, opt => opt.MapFrom(s => s.Course.Credits))
                .ForMember(d => d.StartDate, opt => opt.MapFrom(s => s.Course.StartDate))
                .ForMember(d => d.EndDate, opt => opt.MapFrom(s => s.Course.EndDate))
                .ForMember(d => d.EnrollmentStatus, opt => opt.MapFrom(s => s.Status));
        }
    }

}
