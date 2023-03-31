using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET: api/<StudentsController>
        [HttpGet]
        public IActionResult Get()
        {
            Student student = new Student();
            List<Student> StudentList = student.Read();

            if (StudentList.Count > 0)
            {
                return Ok(StudentList);
            }
            else
            {
                return NotFound("No Student on the system ");
            }
        }

        //GET: api/<StudentsController>
        [HttpGet("studentId/{studentId}")]
        public IActionResult Get(int studentId)
        {
            Student student=new Student();
            student.StudentId = studentId;

            Student res = student.pullSpecificStudent();
            if (res.StudentId == 0)
            {
                return NotFound();

            }
            else
            {
                return Ok(res);
            }
        }


        //POST api/<StudentsController>/5
        [HttpPost]
        public bool Post([FromBody] Student student)
        {         
            return student.Insert();
        }


        // PUT api/<StudentsController>/5
        [HttpPut("studentId/{studentId}")]
        public void Put(int studentId, [FromBody] Student student)
        {
            student.StudentId = studentId;
            student.Update();
        }


        // DELETE api/<StudentsController>/5
        [HttpDelete("groupId/{groupId}")]
        public IActionResult DeleteFromGroupe(int groupId)
        {
            Student s = new Student();
            int num = s.DeleteFromGroupe(groupId);
            if (num == 1)
                return Ok();
            else
                return NotFound();
        }
    }
}
