using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuidesController : ControllerBase
    {
        [HttpPost]
        public bool Post([FromBody] Guide guide)
        {
            return guide.InsertGuide();
        }
    }
}
