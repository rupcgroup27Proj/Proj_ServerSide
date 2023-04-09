using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet("groupId/{groupId}")]
        public List<Student> GetGroupStudents(int groupId)
        {
            Student student = new Student();
            return student.Read(groupId);
        }

        [HttpGet("studentId/{studentId}")]
        public IActionResult Get(int studentId)
        {
            Student student = new Student();
            student.StudentId = studentId;
            Student res = student.pullSpecificStudent();

            return (res.StudentId == 0) ? NotFound() : Ok(res);

        }

        [HttpPost]
        public bool Post([FromBody] Student student)
        {
            return student.Insert();
        }

        [HttpPut("studentId/{studentId}")]
        public IActionResult Put(int studentId, [FromBody] Student student)
        {
            student.StudentId = studentId;
            return (student.Update() == 1) ? Ok(student) : NotFound();
        }

        [HttpDelete("groupId/{groupId}")]
        public IActionResult DeleteFromGroupe(int groupId)
        {
            Student s = new Student();
            int num = s.DeleteFromGroupe(groupId);

            return (num == 1) ? Ok(): NotFound();
        }
    }
}
