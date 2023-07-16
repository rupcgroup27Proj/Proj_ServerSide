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
        public async Task<IActionResult> Post([FromForm] SubmissionModel s)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            string uniqueFileName = GetUniqueFileName("Sub");
            var filePath = Path.Combine(path, "submissions/" + uniqueFileName);
            List<string> imageLinks = new List<string>();
            int check = 0;

            using (var stream = System.IO.File.Create(filePath))
            {
                await s.Document.CopyToAsync(stream);
            }
            imageLinks.Add(uniqueFileName);
            check = SubmissionModel.addSubmission(uniqueFileName, s.Description, s.Id, s.TaskId, s.SubmittedAt);
            Console.WriteLine(check);
            return check == 2 ? Ok() : NotFound();
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

        private string GetUniqueFileName(string fileName)
        {
            string uniqueFileName = fileName;
            int count = 1;
            string path = System.IO.Directory.GetCurrentDirectory();
            string fileExtension = Path.GetExtension(fileName);
            string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);

            while (System.IO.File.Exists(Path.Combine(path, "submissions/" + uniqueFileName)))
            {
                uniqueFileName = $"{fileNameOnly}({count++}).pdf";
            }

            return uniqueFileName;
        }
    }
}
