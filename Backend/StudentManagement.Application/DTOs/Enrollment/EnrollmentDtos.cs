using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Application.DTOs.Enrollment
{
    public class EnrollmentDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<EnrollmentDetailDto> Details { get; set; } = new();
    }

    public class EnrollmentDetailDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public int Credits { get; set; }
        public decimal? Grade { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // Para crear una matricula — acepta multiples cursos en un solo request
    public class CreateEnrollmentDto
    {
        public int StudentId { get; set; }
        public string? Notes { get; set; }
        public List<int> CourseIds { get; set; } = new();  // IDs de los cursos a inscribir
    }

    public class UpdateEnrollmentDto
    {
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public List<UpdateDetailDto> Details { get; set; } = new();
    }

    public class UpdateDetailDto
    {
        public int CourseId { get; set; }
        public decimal? Grade { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // Para la pantalla "Cursos del Estudiante"
    public class StudentCoursesDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<EnrolledCourseDto> Courses { get; set; } = new();
    }

    public class EnrolledCourseDto
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public int Credits { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EnrollmentStatus { get; set; } = string.Empty;
        public decimal? Grade { get; set; }
    }

}
