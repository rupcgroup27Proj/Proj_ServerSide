using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        [HttpPost]
        public bool Post([FromBody] Teacher teacher)
        {
            return teacher.Insert();
        }
    }
}
