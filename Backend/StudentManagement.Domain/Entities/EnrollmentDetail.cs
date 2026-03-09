using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Domain.Entities
{
    public class EnrollmentDetail
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public decimal? Grade { get; set; }
        public string Status { get; set; } = "Enrolled"; // Valores posibles: "Enrolled" | "Passed" | "Failed" | "Withdrawn"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Enrollment Enrollment { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}


