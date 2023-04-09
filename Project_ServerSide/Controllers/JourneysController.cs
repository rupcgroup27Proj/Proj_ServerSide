using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;
using Project_ServerSide.Models.DAL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneysController : ControllerBase
    {
        [HttpGet("GetJourneyList/")]
        public IActionResult GetJourneyList()
        {
            Journey journey = new Journey();
            List<Journey> JourneyList = journey.Read();

            return (JourneyList.Count > 0) ? Ok(JourneyList) : NotFound("No Journey on the system ");
        }

        [HttpGet("GetSpecificJourney/groupId/{groupId}/schoolName/{schoolName}")]
        public IActionResult GetSpecificJourney(int groupId, string schoolName)
        {
            Journey journey = new Journey();
            journey.GroupId = groupId;
            journey.SchoolName = schoolName;
            Journey result = journey.pullSpecificJourney();

            return (result.GroupId == null) ? NotFound() : Ok(result);
        }

        [HttpGet("GetJourneyDatesAndSchoolName/groupId/{groupId}")]
        public object GetJourneyDatesAndSchoolName(int groupId)
        {
            Journey_DBservices dbs = new Journey_DBservices();
            return dbs.GetJourneyDatesAndSchoolName(groupId);
        }

        //read groupId for journey schoolName
        [HttpGet("schoolName/{schoolName}")]
        public IActionResult GetGroupIdBySchoolName(string schoolName)
        {
            JourneyId journeyId = new JourneyId();
            journeyId.SchoolName = schoolName;
            JourneyId result = journeyId.readGroupId();

            return (result.SchoolName == null) ? NotFound() : Ok(result);
        }

        //insert new Journey
        [HttpPost("schoolName/{schoolName}")]
        public int PostSchoolName(string schoolName)
        {
            return Journey.Insert(schoolName);
        }

        // PUT api/<JourneysController>
        [HttpPut("groupId/{groupId}")]
        public IActionResult Put(int groupId, [FromBody] Journey journey)
        {
            journey.GroupId = groupId;
            return (journey.Update() == 1) ? Ok() : NotFound();
        }

        [HttpPut("groupId/{groupId}/startDate/{startDate}/endDate/{endDate}")]
        public IActionResult UpdateJourneyDates(int groupId, DateTime startDate, DateTime endDate)
        {
            Journey_DBservices dbs = new Journey_DBservices();
            return (dbs.UpdateJourneyDates(groupId, startDate, endDate) == 1) ? Ok() : NotFound();
        }
    }
}
