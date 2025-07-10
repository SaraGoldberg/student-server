using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "אירעה שגיאה בעת אחזור הסטודנטים");
            }
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    return NotFound($"סטודנט עם מזהה {id} לא נמצא");
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "אירעה שגיאה בעת אחזור הסטודנט");
            }
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent([FromBody] Student student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdStudent = await _studentService.CreateStudentAsync(student);
                return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.StudentId }, createdStudent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "אירעה שגיאה בעת יצירת הסטודנט");
            }
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedStudent = await _studentService.UpdateStudentAsync(id, student);
                if (updatedStudent == null)
                {
                    return NotFound($"סטודנט עם מזהה {id} לא נמצא");
                }

                return Ok(updatedStudent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "אירעה שגיאה בעת עדכון הסטודנט");
            }
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var isDeleted = await _studentService.DeleteStudentAsync(id);
                if (!isDeleted)
                {
                    return NotFound($"סטודנט עם מזהה {id} לא נמצא");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "אירעה שגיאה בעת מחיקת הסטודנט");
            }
        }
    }
}
