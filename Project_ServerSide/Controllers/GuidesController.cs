using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuidesController : ControllerBase
    {
        private int guideId;

        //GET: api/<GuidesController>
        [HttpGet]

        //public IActionResult Get()
        //{
            
        //}
        //[HttpGet("guidetId/{guideId}/password/{password}")]
        //public IActionResult Get(int guideId, string password)
        //{
        //    Guide guide = new Guide();
        //    guide.GuideId = guideId;
        //    guide.Password = password;
        //    Guide result = guide.Login();
        //    if (guide.GuideId == null)
        //    {
        //        return NotFound();

        //    }
        //    else
        //    {
        //        return Ok(result);
        //    }
        //}


        // GET api/<TeachersController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<TeachersController>
        [HttpPost]
        public bool Post([FromBody] Guide guide)
        {         
            return guide.Insert();

        }

       
    }
}
