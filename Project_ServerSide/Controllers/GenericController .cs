using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController : ControllerBase
    {
        [HttpGet("id/{id}/password/{password}/type/{type}")]
        public GenericUser Get(int id, string password, string type)
        {
            return GenericUser.Login(id, password, type);
        }
    }
}
