using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController : ControllerBase
    {
        private int teacherID;
        private int guideId;
        private int studentId;

        //GET: api/<GenericController>
        [HttpGet]

    
            //login for all users by there types
        
        [HttpGet("Id/{Id}/password/{password}/Type/{type}")]
        public IActionResult Get(int Id, string password,string type)
        {
            if (type == "Teacher")
            {
                Teacher teacher = new Teacher();
                teacher.TeacherId = Id;
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
            else if (type == "Guide")
            {
                Guide guide = new Guide();
                guide.GuideId = Id;
                guide.Password = password;
                Guide result = guide.Login();
                if (guide.GuideId == null)
                {
                    return NotFound();

                }
                else
                {
                    return Ok(result);
                }
            }
            else if (type == "Student")
            {
                Student student = new Student();
                student.StudentId = Id;
                student.Password = password;
                Student result = student.Login();
                if (result.StudentId == null)
                {
                    return NotFound();

                }
                else
                {
                    return Ok(result);
                }
            }
            else
                return null;
        }
            

    }
}
