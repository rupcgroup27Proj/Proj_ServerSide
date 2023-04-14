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


        [HttpGet("studentId/{studentId}")] ///////למחוק 
        public IActionResult GetSpecificStudent(int studentId)
        {
            Student student = new Student();
            student.StudentId = studentId;
            Student res = student.GetSpecificStudent();

            return (res.StudentId == 0) ? NotFound() : Ok(res);
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


        [HttpDelete("studentId/{studentId}")]
        public IActionResult Delete(int studentId)
        {
            Student s = new Student();
            int num = s.DeleteFromGroup(studentId);

            return (num == 1) ? Ok() : NotFound();
        }
    }
}
