using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;
using Project_ServerSide.Models.DAL;
using Project_ServerSide.Models.SmartQuestionnaires;

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

        // POST api/<QuestionnairesController>
        [HttpPost("groupId/{groupId}/json/{questionnaire}")]
        public void Post(int groupId, string questionnaire)
        {
            Questionnaire_DBservices dbs = new Questionnaire_DBservices();
            dbs.InsertNewQuestionnaire(groupId, questionnaire);
        }

        // PUT api/<QuestionnairesController>/5
        [HttpPut("studentId/{studentId}")]
        public void Put(int studentId, [FromBody] List<Tag> tags)
        {
            Questionnaire.updateStudentTags(studentId, tags);
        }

        // DELETE api/<QuestionnairesController>/5
        [HttpDelete("id/{id}")]
        public void Delete(int id)
        {
        }
    }
}
