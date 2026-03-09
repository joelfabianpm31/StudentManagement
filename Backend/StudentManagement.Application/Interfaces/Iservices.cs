using System;
using System.Collections.Generic;
using System.Text;
using StudentManagement.Application.DTOs.Course;
using StudentManagement.Application.DTOs.Enrollment;
using StudentManagement.Application.DTOs.Student;

namespace StudentManagement.Application.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllAsync();
        Task<StudentDto> GetByIdAsync(int id);
        Task<StudentDto> CreateAsync(CreateStudentDto dto);
        Task<StudentDto> UpdateAsync(int id, UpdateStudentDto dto);
        Task DeleteAsync(int id);
        Task<StudentCoursesDto> GetStudentCoursesAsync(int studentId);
    }

    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<CourseDto> GetByIdAsync(int id);
        Task<CourseDto> CreateAsync(CreateCourseDto dto);
        Task<CourseDto> UpdateAsync(int id, UpdateCourseDto dto);
        Task DeleteAsync(int id);
    }

    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDto>> GetAllAsync();
        Task<EnrollmentDto> GetByIdAsync(int id);
        Task<IEnumerable<EnrollmentDto>> GetByStudentAsync(int studentId);
        Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto);
        Task<EnrollmentDto> UpdateAsync(int id, UpdateEnrollmentDto dto);
        Task DeleteAsync(int id);
    }

}
