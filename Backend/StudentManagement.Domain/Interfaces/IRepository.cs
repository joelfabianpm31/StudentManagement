using StudentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(int id);

    }
    public interface IStudentRepository : IRepository<Student>
    {
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task<bool> DocumentExistsAsync(string document, int? excludeId = null);
        Task<Student?> GetWithEnrollmentsAsync(int id);
    }
    public interface ICourseRepository : IRepository<Course>
    {
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        Task<int> GetEnrolledCountAsync(int courseId);
    }
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<Enrollment?> GetWithDetailsAsync(int id);
        Task<IEnumerable<Enrollment>> GetByStudentAsync(int studentId);
        Task<bool> StudentAlreadyEnrolledInCourseAsync(
            int studentId, int courseId, int? excludeEnrollmentId = null);
    }
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        ICourseRepository Courses { get; }
        IEnrollmentRepository Enrollments { get; }
        Task<int> SaveChangesAsync();
    }
}
