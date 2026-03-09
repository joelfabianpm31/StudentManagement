using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces;
using StudentManagement.Infrastructure.Data;

namespace StudentManagement.Infrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<Course>> GetAllAsync()
            => await _dbSet
                   .OrderBy(c => c.Name)
                   .ToListAsync();

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
            => await _dbSet.AnyAsync(c =>
                   c.Code == code &&
                   (!excludeId.HasValue || c.Id != excludeId.Value));

        public async Task<int> GetEnrolledCountAsync(int courseId)
            => await _context.EnrollmentDetails
                   .Include(ed => ed.Enrollment)
                   .CountAsync(ed =>
                       ed.CourseId == courseId &&
                       ed.Enrollment.Status == "Active");
    }
}

