using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Application.DTOs.Course
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int MaxStudents { get; set; }
        public int EnrolledCount { get; set; }  // cuantos hay inscritos actualmente
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateCourseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int MaxStudents { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UpdateCourseDto : CreateCourseDto
    {
        public bool IsActive { get; set; }
    }

}
