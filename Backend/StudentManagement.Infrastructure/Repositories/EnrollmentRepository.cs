using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Interfaces;
using StudentManagement.Infrastructure.Data;

namespace StudentManagement.Infrastructure.Repositories;

public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(ApplicationDbContext context) : base(context) { }

    // GetAll con todas las relaciones cargadas
    public override async Task<IEnumerable<Enrollment>> GetAllAsync()
        => await _dbSet
               .Include(e => e.Student)
               .Include(e => e.EnrollmentDetails)
                   .ThenInclude(ed => ed.Course)
               .OrderByDescending(e => e.EnrollmentDate)
               .ToListAsync();

    // Obtiene una matricula con todos sus detalles y relaciones
    public async Task<Enrollment?> GetWithDetailsAsync(int id)
        => await _dbSet
               .Include(e => e.Student)
               .Include(e => e.EnrollmentDetails)
                   .ThenInclude(ed => ed.Course)
               .FirstOrDefaultAsync(e => e.Id == id);

    // Obtiene todas las matriculas de un estudiante especifico
    public async Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId)
        => await _dbSet
               .Include(e => e.Student)
               .Include(e => e.EnrollmentDetails)
                   .ThenInclude(ed => ed.Course)
               .Where(e => e.StudentId == studentId)
               .OrderByDescending(e => e.EnrollmentDate)
               .ToListAsync();

    // Verifica si el estudiante ya esta inscrito en ese curso (en alguna matricula activa)
    public async Task<bool> StudentAlreadyEnrolledInCourseAsync(
        int studentId, int courseId, int? excludeEnrollmentId = null)
        => await _dbSet
               .Where(e =>
                   e.StudentId == studentId &&
                   e.Status == "Active" &&
                   (!excludeEnrollmentId.HasValue || e.Id != excludeEnrollmentId.Value))
               .AnyAsync(e =>
                   e.EnrollmentDetails.Any(ed => ed.CourseId == courseId));
}

