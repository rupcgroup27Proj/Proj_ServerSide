using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneysController : ControllerBase
    {
        //GET: api/<JourneysController>
        [HttpGet]

        public IActionResult Get()
        {
            Journey journey = new Journey();
            List<Journey> JourneyList = journey.Read();

            if (JourneyList.Count > 0)
            {
                return Ok(JourneyList);
            }
            else
            {
                return NotFound("No Journey on the system ");
            }
        }
        [HttpGet("groupId/{groupId}/schoolName/{schoolName}")]

        public IActionResult Get(int groupId, string schoolName)
        {
            Journey journey = new Journey();
            journey.GroupId = groupId;
            journey.SchoolName = schoolName;
            Journey result = journey.pullSpecificJourney();
            if (result.GroupId == null)
            {
                return NotFound();

            }
            else
            {
                return Ok(result);
            }
        }

        //insert new Journey
        [HttpPost("schoolName/{schoolName}")]
        public void Post(string schoolName)
        {         
             Journey.Insert(schoolName);

        }
        [HttpPut("{groupId}")]
        public void Put(int groupId, [FromBody] Journey journey)
        {
            journey.GroupId = groupId;
            journey.Update();
        }
        // PUT api/<StudentsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] Student student)
        //{
        //    //student.Id = id;
        //    //student.Update();
        //}

        //// DELETE api/<StudentsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
