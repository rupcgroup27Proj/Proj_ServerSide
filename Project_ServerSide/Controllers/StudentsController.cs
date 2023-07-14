using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


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
            return student.GetGroupStudents(groupId);
        }

        [HttpGet("gId/{gId}")]
        public List<object> GetTokens(int gId)
        {
            return Student.GetTokens(gId);
        }

        [HttpPost]
        public bool Post([FromBody] Student student)
        {
            return student.InsertStudent();
        }


        [HttpPut("studentId/{studentId}")]
        public IActionResult Put(int studentId, [FromBody] Student student)
        {
            student.StudentId = studentId;
            return (student.UpdateStudent() == 1) ? Ok(student) : NotFound();
        }

        [HttpPut("studentId/{studentId}/token/{token}")]
        public IActionResult Put(int studentId, string token)
        {
            return (Student.UpdateToken(studentId, token) == 1) ? Ok() : NotFound();
        }

        [HttpDelete("studentId/{studentId}")]
        public IActionResult Delete(int studentId)
        {
            Student s = new Student();
            int num = s.DeleteFromGroup(studentId);

            return (num == 1) ? Ok() : NotFound();
        }
    }
}
