using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;
using System.IO;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {

        [HttpGet("taskId/{taskId}")]
        public IActionResult Get(int taskId)
        {
            Tasks tasks = new Tasks();
            List<Tasks> TasksList = tasks.GetTaskByID(taskId);

            return (TasksList.Count > 0) ? Ok(TasksList) : NotFound("No Tasks on the system");
        }


        [HttpGet("groupId/{groupId}")]
        public List<Tasks> ReadTaskList(int groupId)
        {
            Tasks tasks = new Tasks();
            return tasks.ReadTaskList(groupId);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] PdfModel pdf)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            string uniqueFileName = GetUniqueFileName(pdf.Name);
            var filePath = Path.Combine(path, "pdfs/" + uniqueFileName);
            List<string> imageLinks = new List<string>();
            int check = 0;

            using (var stream = System.IO.File.Create(filePath))
            {
                await pdf.Document.CopyToAsync(stream);
            }
            imageLinks.Add(uniqueFileName);
            check = PdfModel.addPdf(uniqueFileName, pdf.Description, pdf.Date, pdf.Name, pdf.GroupId);
            return check == 2 ? Ok() : NotFound();
        }


        private string GetUniqueFileName(string fileName)
        {
            string uniqueFileName = fileName + ".pdf";
            int count = 1;
            string path = System.IO.Directory.GetCurrentDirectory();
            string fileExtension = Path.GetExtension(fileName);
            string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);

            while (System.IO.File.Exists(Path.Combine(path, "pdfs/" + uniqueFileName)))
            {
                uniqueFileName = $"{fileNameOnly}({count++}){fileExtension}";
            }

            return uniqueFileName;
        }
    }
}
