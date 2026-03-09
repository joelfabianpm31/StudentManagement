using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentManagement.Application.DTOs.Enrollment;
using StudentManagement.Application.DTOs.Student;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Exceptions;
using StudentManagement.Domain.Interfaces;



namespace StudentManagement.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IUnitOfWork uow, IMapper mapper, ILogger<StudentService> logger)
        { _uow = uow; _mapper = mapper; _logger = logger; }

        public async Task<IEnumerable<StudentDto>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo lista de estudiantes");
            var students = await _uow.Students.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto> GetByIdAsync(int id)
        {
            var student = await _uow.Students.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Student), id);
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
        {
            _logger.LogInformation("Creando estudiante: {Email}", dto.Email);

            if (await _uow.Students.EmailExistsAsync(dto.Email))
                throw new BusinessRuleException($"El email '{dto.Email}' ya esta registrado.");

            if (await _uow.Students.DocumentExistsAsync(dto.DocumentNumber))
                throw new BusinessRuleException(
                    $"El documento '{dto.DocumentNumber}' ya existe en el sistema.");

            var student = _mapper.Map<Student>(dto);
            var created = await _uow.Students.AddAsync(student);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Estudiante creado con Id {Id}", created.Id);
            return _mapper.Map<StudentDto>(created);
        }

        public async Task<StudentDto> UpdateAsync(int id, UpdateStudentDto dto)
        {
            var student = await _uow.Students.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Student), id);

            if (await _uow.Students.EmailExistsAsync(dto.Email, id))
                throw new BusinessRuleException($"El email '{dto.Email}' ya esta en uso.");

            if (await _uow.Students.DocumentExistsAsync(dto.DocumentNumber, id))
                throw new BusinessRuleException(
                    $"El documento '{dto.DocumentNumber}' ya pertenece a otro estudiante.");

            _mapper.Map(dto, student);
            await _uow.Students.UpdateAsync(student);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Estudiante {Id} actualizado", id);
            return _mapper.Map<StudentDto>(student);
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _uow.Students.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Student), id);

            await _uow.Students.DeleteAsync(student);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Estudiante {Id} eliminado", id);
        }

        public async Task<StudentCoursesDto> GetStudentCoursesAsync(int studentId)
        {
            var student = await _uow.Students.GetWithEnrollmentsAsync(studentId)
                ?? throw new NotFoundException(nameof(Student), studentId);

            // Solo cursos de matriculas activas
            var enrolledCourses = student.Enrollments
                .Where(e => e.Status == "Active")
                .SelectMany(e => e.EnrollmentDetails)
                .Select(ed => _mapper.Map<EnrolledCourseDto>(ed))
                .ToList();

            return new StudentCoursesDto
            {
                StudentId = student.Id,
                StudentName = student.FullName,
                Email = student.Email,
                Courses = enrolledCourses
            };
        }
    }

}
