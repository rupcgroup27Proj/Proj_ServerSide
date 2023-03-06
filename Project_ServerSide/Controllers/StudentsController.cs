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

        [HttpGet("studentId/{studentId}")]

        public IActionResult Get(int studentId)
        {
            Student student=new Student();
            student.StudentId = studentId;

            Student res = student.pullSpecificStudent();
            if (res.StudentId == null)
            {
                return NotFound();

            }
            else
            {
                return Ok(res);
            }
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{studentId}")]
        public void Put(int studentId, [FromBody] Student student)
        {
            student.StudentId = studentId;
            student.Update();
        }
        //[HttpGet("studentId/{studentId}/password/{password}")]
        //public IActionResult Get(int studentId, string password)
        //{
        //    Student student = new Student();
        //    student.StudentId =studentId;
        //    student.Password = password;
        //    Student result = student.Login();
        //    if (result.StudentId == null)
        //    {
        //        return NotFound();

        //    }
        //    else
        //    {
        //        return Ok(result);
        //    }
        //}

        //insert new student
        [HttpPost]
        public bool Post([FromBody] Student student)
        {         
            return student.Insert();

        }

        // PUT api/<StudentsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] Student student)
        //{
        //    //student.Id = id;
        //    //student.Update();
        //}

        //// DELETE api/<StudentsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
