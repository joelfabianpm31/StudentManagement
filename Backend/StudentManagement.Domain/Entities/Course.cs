using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Domain.Entities
{
public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public int Credits { get; set; }
        public int MaxStudents { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }   
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;


        public ICollection<EnrollmentDetail> EnrollmentDetails { get; set; } = new List<EnrollmentDetail>();


    }
}
