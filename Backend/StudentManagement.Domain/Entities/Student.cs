using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;


        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public string FullName => $"{FirstName} {LastName}";
    }
}
