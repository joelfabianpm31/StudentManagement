using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Application.DTOs.Student
{
    // Lo que devuelve la API al consultar un estudiante
    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int EnrollmentCount { get; set; }
    }

    // Lo que recibe la API para crear un estudiante
    public class CreateStudentDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }

    // Lo que recibe la API para actualizar un estudiante (extiende CreateStudentDto)
    public class UpdateStudentDto : CreateStudentDto
    {
        public bool IsActive { get; set; }
    }
}

