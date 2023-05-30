using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        [HttpGet("taskId/{taskId}")]
        public List <Submission> Get(int taskId)
        {
            return Submission.ReadSubList(taskId);
        }


        [HttpPost]
        public bool Post([FromBody] Submission Submission)
        {
            return Submission.SubmitTaskByStudent();
        }


        [HttpPut("submissionId/{submissionId}")]
        public IActionResult Put(int submissionId, [FromBody] Submission Submission)
        {
            Submission.SubmissionId = submissionId;
            return (Submission.UpdateSubmittion() == 1) ? Ok(Submission) : NotFound();

        }


        [HttpDelete("submissionId/{submissionId}")]
        public IActionResult Delete(int submissionId)
        {
            Submission Submission = new Submission();
            int s = Submission.DeleteSubmittion(submissionId);

            return Ok(s);
        }
    }
}
