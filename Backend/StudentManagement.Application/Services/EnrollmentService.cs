using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentManagement.Application.DTOs.Enrollment;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Exceptions;
using StudentManagement.Domain.Interfaces;



namespace StudentManagement.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<EnrollmentService> _logger;

        public EnrollmentService(IUnitOfWork uow, IMapper mapper,
            ILogger<EnrollmentService> logger)
        { _uow = uow; _mapper = mapper; _logger = logger; }

        public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
        {
            var enrollments = await _uow.Enrollments.GetAllAsync();
            return _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
        }

        public async Task<EnrollmentDto> GetByIdAsync(int id)
        {
            var enrollment = await _uow.Enrollments.GetWithDetailsAsync(id)
                ?? throw new NotFoundException(nameof(Enrollment), id);
            return _mapper.Map<EnrollmentDto>(enrollment);
        }

        public async Task<IEnumerable<EnrollmentDto>> GetByStudentAsync(int studentId)
        {
            if (!await _uow.Students.ExistsAsync(studentId))
                throw new NotFoundException(nameof(Student), studentId);
            var enrollments = await _uow.Enrollments.GetByStudentAsync(studentId);
            return _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
        }

        public async Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto)
        {
            _logger.LogInformation("Creando matricula para StudentId {Id}", dto.StudentId);

            if (!await _uow.Students.ExistsAsync(dto.StudentId))
                throw new NotFoundException(nameof(Student), dto.StudentId);

            if (dto.CourseIds == null || !dto.CourseIds.Any())
                throw new BusinessRuleException("Debes seleccionar al menos un curso.");

            var details = new List<EnrollmentDetail>();

            // Valida cada curso antes de crear la matricula
            foreach (var courseId in dto.CourseIds.Distinct())
            {
                var course = await _uow.Courses.GetByIdAsync(courseId)
                    ?? throw new NotFoundException(nameof(Course), courseId);

                if (!course.IsActive)
                    throw new BusinessRuleException(
                        $"El curso '{course.Name}' no esta activo.");

                var enrolled = await _uow.Courses.GetEnrolledCountAsync(courseId);
                if (enrolled >= course.MaxStudents)
                    throw new BusinessRuleException(
                        $"El curso '{course.Name}' alcanzo su cupo maximo ({course.MaxStudents}).");

                if (await _uow.Enrollments.StudentAlreadyEnrolledInCourseAsync(
                        dto.StudentId, courseId))
                    throw new BusinessRuleException(
                        $"El estudiante ya esta inscrito en el curso '{course.Name}'.");

                details.Add(new EnrollmentDetail
                {
                    CourseId = courseId,
                    Status = "Enrolled"
                });
            }

            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                Notes = dto.Notes,
                Status = "Active",
                EnrollmentDate = DateTime.UtcNow,
                EnrollmentDetails = details
            };

            var created = await _uow.Enrollments.AddAsync(enrollment);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Matricula creada con Id {Id}", created.Id);
            return await GetByIdAsync(created.Id);
        }

        public async Task<EnrollmentDto> UpdateAsync(int id, UpdateEnrollmentDto dto)
        {
            var enrollment = await _uow.Enrollments.GetWithDetailsAsync(id)
                ?? throw new NotFoundException(nameof(Enrollment), id);

            enrollment.Status = dto.Status;
            enrollment.Notes = dto.Notes;

            // Actualiza notas y estado de cada detalle
            foreach (var detail in dto.Details)
            {
                var ed = enrollment.EnrollmentDetails
                    .FirstOrDefault(x => x.CourseId == detail.CourseId);
                if (ed is not null)
                {
                    ed.Grade = detail.Grade;
                    ed.Status = detail.Status;
                }
            }

            await _uow.Enrollments.UpdateAsync(enrollment);
            await _uow.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var enrollment = await _uow.Enrollments.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Enrollment), id);
            await _uow.Enrollments.DeleteAsync(enrollment);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Matricula {Id} eliminada", id);
        }
    }

}
