using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmitionController : ControllerBase
    {
        [HttpGet("taskId/{taskId}")]
        public IActionResult Get(int taskId)
        {
            Submittion submittion = new Submittion();
            List<Submittion> SubList = submittion.ReadSubList(taskId);

            return Ok(SubList);
        }



        [HttpPost]
        public bool Post([FromBody] Submittion submittion)
        {
            return submittion.SubmitTaskByStudent();
        }


        [HttpPut("submissionId/{submissionId}")]
        public IActionResult Put(int submissionId, [FromBody] Submittion submittion)
        {
            submittion.SubmissionId = submissionId;
            return (submittion.UpdateSubmittion() == 1) ? Ok(submittion) : NotFound();

        }


        [HttpDelete("submissionId/{submissionId}")]
        public IActionResult Delete(int submissionId)
        {
            Submittion submittion = new Submittion();
            int s = submittion.DeleteSubmittion(submissionId);

            return Ok(s);
        }
    }
}
