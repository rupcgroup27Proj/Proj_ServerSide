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
        [HttpGet("GetJourneyList/")]
        public IActionResult GetJourneyList()
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


        //GET: api/<JourneysController>
        [HttpGet("GetSpecificJourney/groupId/{groupId}/schoolName/{schoolName}")]
        public IActionResult GetSpecificJourney(int groupId, string schoolName)
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

        //read groupId for journey schoolName
        [HttpGet("schoolName/{schoolName}")]
        public IActionResult GetGroupIdBySchoolName(string schoolName)
        {
            JourneyId journeyId = new JourneyId();
            journeyId.SchoolName = schoolName;
            JourneyId result = journeyId.readGroupId();
            if (result.SchoolName == null)
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
        public void PostSchoolName(string schoolName)
        {         
              Journey.Insert(schoolName);

        }

        // PUT api/<JourneysController>
        [HttpPut("groupId/{groupId}")]
        public void Put(int groupId, [FromBody] Journey journey)
        {
            journey.GroupId = groupId;
            journey.Update();
        }
     
    }
}
