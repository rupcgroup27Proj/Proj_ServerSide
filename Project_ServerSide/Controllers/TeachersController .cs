using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;
using System;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] Teacher teacher)
        {
            int res = teacher.InsertTeacher();
            if (res == 2)
                return Ok(2);
            if (res == 1)
                return Ok(1);
            else
                return StatusCode(500, "Could not associate a teacher to the delegation");
        }
    }
}
