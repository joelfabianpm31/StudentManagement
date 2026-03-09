using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Common;
using StudentManagement.Application.DTOs.Course;
using StudentManagement.Application.Interfaces;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseService service,
            ILogger<CoursesController> logger)
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
            var course = await _service.GetByIdAsync(id);
            return Ok(ApiResponse<CourseDto>.Ok(course));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            var course = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = course.Id },
                ApiResponse<CourseDto>.Ok(course, "Curso creado exitosamente"));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,
            [FromBody] UpdateCourseDto dto)
        {
            var course = await _service.UpdateAsync(id, dto);
            return Ok(ApiResponse<CourseDto>.Ok(course, "Curso actualizado"));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<object>.Ok(null!, "Curso eliminado exitosamente"));
        }
    }

}
