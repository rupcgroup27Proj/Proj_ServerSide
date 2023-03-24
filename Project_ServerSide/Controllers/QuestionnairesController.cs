using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models.DAL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnairesController : ControllerBase
    {
        // GET: api/<QuestionnairesController>
        [HttpGet("groupId/{groupId}")]
        public IActionResult GetAllQuestionnaires(int groupId)
        {
            Questionnaire_DBservices dbs = new Questionnaire_DBservices();
            return Ok(dbs.GetAllQuestionnaires(groupId));
        }

        // GET api/<QuestionnairesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<QuestionnairesController>
        [HttpPost("groupId/{groupId}/json/{questionnaire}")]
        public void Post(int groupId, string questionnaire)
        {
            Questionnaire_DBservices dbs = new Questionnaire_DBservices();
            dbs.InsertNewQuestionnaire(groupId, questionnaire);
        }

        // PUT api/<QuestionnairesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<QuestionnairesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
