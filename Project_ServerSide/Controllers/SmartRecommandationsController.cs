using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models.Algorithm;
using System.Reflection.Metadata.Ecma335;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmartRecommandationsController : ControllerBase
    {
        // GET: api/<SmartRecommandationsController>
        [HttpGet]
        public void Get()
        {
            
        }



        // GET api/<SmartRecommandationsController>/5
        [HttpGet("studentId/{studentId}")]
        public List<string> GetStudentRecommandations(int studentId)
        {
            bool isGettingTags = true;
            return Algorithm.RunAlgorithm(isGettingTags, studentId);
        }

        // POST api/<SmartRecommandationsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SmartRecommandationsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SmartRecommandationsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
