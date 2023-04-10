using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;
using Project_ServerSide.Models.DAL;
using Project_ServerSide.Models.SmartQuestionnaires;
using System.Text.Json.Nodes;

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnairesController : ControllerBase
    {
        [HttpGet("groupId/{groupId}")]
        public IActionResult GetAllQuestionnaires(int groupId)
        {
            Questionnaire_DBservices dbs = new Questionnaire_DBservices();
            return Ok(dbs.GetAllQuestionnaires(groupId));
        }


        [HttpPost("groupId/{groupId}")]
        public void Post(int groupId, [FromBody] JsonObject questionnaire) 
        {
            Questionnaire_DBservices dbs = new Questionnaire_DBservices();
            dbs.InsertNewQuestionnaire(groupId, questionnaire);
        }


        [HttpPut("studentId/{studentId}")]
        public void UpdateStudentTags(int studentId, [FromBody] List<Tag> tags)
        {
            Questionnaire.updateStudentTags(studentId, tags);
        }


        [HttpDelete("questionnaireId/{questionnaireId}")]
        public void DeleteQuestionnaire(int questionnaireId)
        {
            Questionnaire_DBservices dbs = new Questionnaire_DBservices();
            dbs.DeleteQuestionnaire(questionnaireId);
        }
    }
}
