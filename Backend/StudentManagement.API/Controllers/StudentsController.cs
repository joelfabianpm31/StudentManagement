using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Common;
using StudentManagement.Application.DTOs.Student;
using StudentManagement.Application.Interfaces;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService service,
            ILogger<StudentsController> logger)
        { _service = service; _logger = logger; }

        /// <summary>Obtiene la lista de todos los estudiantes activos</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(ApiResponse<object>.Ok(data,
                $"{data.Count()} estudiantes encontrados"));
        }

        /// <summary>Obtiene un estudiante por su ID</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _service.GetByIdAsync(id);
            return Ok(ApiResponse<StudentDto>.Ok(student));
        }

        /// <summary>Obtiene los cursos en los que esta inscrito el estudiante</summary>
        [HttpGet("{id:int}/courses")]
        public async Task<IActionResult> GetCourses(int id)
        {
            var result = await _service.GetStudentCoursesAsync(id);
            return Ok(ApiResponse<object>.Ok(result));
        }

        /// <summary>Crea un nuevo estudiante</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
        {
            var student = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = student.Id },
                ApiResponse<StudentDto>.Ok(student, "Estudiante creado exitosamente"));
        }

        /// <summary>Actualiza los datos de un estudiante</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,
            [FromBody] UpdateStudentDto dto)
        {
            var student = await _service.UpdateAsync(id, dto);
            return Ok(ApiResponse<StudentDto>.Ok(student,
                "Estudiante actualizado correctamente"));
        }

        /// <summary>Elimina un estudiante</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<object>.Ok(null!,
                "Estudiante eliminado exitosamente"));
        }
    }

}
