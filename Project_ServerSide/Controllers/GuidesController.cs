using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuidesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] Guide guide)
        {
            int res = guide.InsertGuide();
            if (res == 2)
                return Ok(2);
            if (res == 1)
                return Ok(1);
            else 
                return StatusCode(500, "Could not associate a guide to the delegation");
        }
    }
}
