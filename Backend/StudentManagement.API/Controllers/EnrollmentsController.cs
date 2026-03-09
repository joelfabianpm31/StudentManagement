using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Common;
using StudentManagement.Application.DTOs.Enrollment;
using StudentManagement.Application.Interfaces;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _service;
        private readonly ILogger<EnrollmentsController> _logger;

        public EnrollmentsController(IEnrollmentService service,
            ILogger<EnrollmentsController> logger)
        { _service = service; _logger = logger; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(ApiResponse<object>.Ok(data));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var enrollment = await _service.GetByIdAsync(id);
            return Ok(ApiResponse<EnrollmentDto>.Ok(enrollment));
        }

        // GET api/enrollments/student/5 — todas las matriculas de un estudiante
        [HttpGet("student/{studentId:int}")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var data = await _service.GetByStudentAsync(studentId);
            return Ok(ApiResponse<object>.Ok(data));
        }

        // POST api/enrollments — crea matricula con multiples cursos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentDto dto)
        {
            var enrollment = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = enrollment.Id },
                ApiResponse<EnrollmentDto>.Ok(enrollment,
                    $"Matricula creada con {enrollment.Details.Count} curso(s)"));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,
            [FromBody] UpdateEnrollmentDto dto)
        {
            var enrollment = await _service.UpdateAsync(id, dto);
            return Ok(ApiResponse<EnrollmentDto>.Ok(enrollment, "Matricula actualizada"));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<object>.Ok(null!, "Matricula eliminada"));
        }
    }

}
