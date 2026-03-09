using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces;
using StudentManagement.Infrastructure.Data;


namespace StudentManagement.Infrastructure.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context) { }
        public override async Task<IEnumerable<Student>> GetAllAsync()
            => await _dbSet
                   .Include(s => s.Enrollments)
                   .Where(s => s.IsActive)
                   .OrderBy(s => s.LastName)
                   .ThenBy(s => s.FirstName)
                   .ToListAsync();
        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
            => await _dbSet.AnyAsync(s =>
                   s.Email == email &&
                   (!excludeId.HasValue || s.Id != excludeId.Value));

        public async Task<bool> DocumentExistsAsync(string document, int? excludeId = null)
            => await _dbSet.AnyAsync(s =>
                   s.DocumentNumber == document &&
                   (!excludeId.HasValue || s.Id != excludeId.Value));

        public async Task<Student?> GetWithEnrollmentsAsync(int id)
            => await _dbSet
                   .Include(s => s.Enrollments)
                       .ThenInclude(e => e.EnrollmentDetails)
                           .ThenInclude(ed => ed.Course)
                   .FirstOrDefaultAsync(s => s.Id == id);
    }
}
