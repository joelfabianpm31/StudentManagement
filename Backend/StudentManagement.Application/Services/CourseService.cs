using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentManagement.Application.DTOs.Course;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Exceptions;
using StudentManagement.Domain.Interfaces;


namespace StudentManagement.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;

        public CourseService(IUnitOfWork uow, IMapper mapper, ILogger<CourseService> logger)
        { _uow = uow; _mapper = mapper; _logger = logger; }

        public async Task<IEnumerable<CourseDto>> GetAllAsync()
        {
            var courses = await _uow.Courses.GetAllAsync();
            var dtos = _mapper.Map<List<CourseDto>>(courses);
            // Carga el conteo de inscritos para cada curso
            foreach (var dto in dtos)
                dto.EnrolledCount = await _uow.Courses.GetEnrolledCountAsync(dto.Id);
            return dtos;
        }

        public async Task<CourseDto> GetByIdAsync(int id)
        {
            var course = await _uow.Courses.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Course), id);
            var dto = _mapper.Map<CourseDto>(course);
            dto.EnrolledCount = await _uow.Courses.GetEnrolledCountAsync(id);
            return dto;
        }

        public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
        {
            _logger.LogInformation("Creando curso: {Code}", dto.Code);

            if (await _uow.Courses.CodeExistsAsync(dto.Code))
                throw new BusinessRuleException($"El codigo de curso '{dto.Code}' ya existe.");

            if (dto.EndDate <= dto.StartDate)
                throw new BusinessRuleException("La fecha de fin debe ser mayor a la de inicio.");

            if (dto.MaxStudents <= 0)
                throw new BusinessRuleException("El cupo maximo debe ser mayor a 0.");

            var course = _mapper.Map<Course>(dto);
            var created = await _uow.Courses.AddAsync(course);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Curso creado con Id {Id}", created.Id);
            return await GetByIdAsync(created.Id);
        }

        public async Task<CourseDto> UpdateAsync(int id, UpdateCourseDto dto)
        {
            var course = await _uow.Courses.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Course), id);

            if (await _uow.Courses.CodeExistsAsync(dto.Code, id))
                throw new BusinessRuleException($"El codigo '{dto.Code}' ya esta en uso.");

            _mapper.Map(dto, course);
            await _uow.Courses.UpdateAsync(course);
            await _uow.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _uow.Courses.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Course), id);

            var enrolledCount = await _uow.Courses.GetEnrolledCountAsync(id);
            if (enrolledCount > 0)
                throw new BusinessRuleException(
                    $"No se puede eliminar el curso. Tiene {enrolledCount} estudiante(s) inscrito(s).");

            await _uow.Courses.DeleteAsync(course);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Curso {Id} eliminado", id);
        }
    }

}
