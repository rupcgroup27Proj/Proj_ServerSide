using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;
using Project_ServerSide.Models.DAL;


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
            List<Journey> JourneyList = journey.GetJourneyList();
            return (JourneyList.Count > 0) ? Ok(JourneyList) : NotFound("No Journey on the system ");
        }

        [HttpGet("GetUserJourneys/userId/{userId}/userType/{userType}")]
        public IActionResult GetUserJourneyList(int userId, string userType)
        {
            Journey journey = new Journey();
            List<Journey> JourneyList = journey.GetUserJourneyList(userId, userType);
            return (JourneyList.Count > 0) ? Ok(JourneyList) : NotFound("No Journey on the system");
        }

        [HttpGet("GetJourneyDatesAndSchoolName/groupId/{groupId}")]
        public object GetJourneyDatesAndSchoolName(int groupId)
        {
            Journey_DBservices dbs = new Journey_DBservices();
            return dbs.GetJourneyDatesAndSchoolName(groupId);
        }

    
        [HttpPost("schoolName/{schoolName}")]
        public int Post(string schoolName)
        {
            return Journey.InsertSchoolName(schoolName);
        }


        [HttpPut("groupId/{groupId}/startDate/{startDate}/endDate/{endDate}")] 
        public IActionResult UpdateJourneyDates(int groupId, DateTime startDate, DateTime endDate)
        {
            return (Journey.UpdateJourneyDates(groupId, startDate, endDate) == 1) ? Ok() : NotFound();
        }
    }
}
