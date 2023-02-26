using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private int teacherID;

        //GET: api/<TeachersController>
        [HttpGet]

        //public IActionResult Get()
        //{
            
        //}
        [HttpGet("teachertId/{teacherId}/password/{password}")]
        public IActionResult Get(int teacherId, string password)
        {
            Teacher teacher = new Teacher();
            teacher.TeacherId = teacherId;
            teacher.Password = password;
            Teacher result = teacher.Login();
            if (teacher.TeacherId == null)
            {
                return NotFound();

            }
            else
            {
                return Ok(result);
            }
        }




        // GET api/<TeachersController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<TeachersController>
        [HttpPost]
        public bool Post([FromBody] Teacher teacher)
        {         
            return teacher.Insert();

        }

        // PUT api/<TeachersController>/5
        //[HttpPut("{id}")]
        
        //// DELETE api/<TeachersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
