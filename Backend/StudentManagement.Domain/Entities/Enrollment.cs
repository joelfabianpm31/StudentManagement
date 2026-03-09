using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Domain.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Active"; // Valores posibles: "Active" | "Completed" | "Cancelled"
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public Student Student { get; set; } = null!;
        public ICollection<EnrollmentDetail> EnrollmentDetails { get; set; } = new List<EnrollmentDetail>();
    }
}
