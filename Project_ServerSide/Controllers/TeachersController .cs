using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        // POST api/<TeachersController>
        [HttpPost]
        public bool Post([FromBody] Teacher teacher)
        {         
            return teacher.Insert();

        }
    }
}
