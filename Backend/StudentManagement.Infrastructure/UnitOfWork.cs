using System;
using System.Collections.Generic;
using System.Text;
using StudentManagement.Domain.Interfaces;
using StudentManagement.Infrastructure.Data;
using StudentManagement.Infrastructure.Repositories;

namespace StudentManagement.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    // Inicializacion lazy — solo se instancian cuando se acceden
    private IStudentRepository? _students;
    private ICourseRepository? _courses;
    private IEnrollmentRepository? _enrollments;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IStudentRepository Students
        => _students ??= new StudentRepository(_context);

    public ICourseRepository Courses
        => _courses ??= new CourseRepository(_context);

    public IEnrollmentRepository Enrollments
        => _enrollments ??= new EnrollmentRepository(_context);

    // Todos los cambios pendientes se guardan en UNA sola transaccion
    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
        => _context.Dispose();
}
