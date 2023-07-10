using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapsController : ControllerBase
    {
        // GET: api/<MapsController>
        [HttpGet("studentId/{studentId}")]
        public object Get(int studentId)
        {
            return Map.getStudentMapComponents(studentId);
        }

        [HttpPost("studentId/{studentId}/locationId/{locationId}")]
        public async Task<IActionResult> Post([FromForm] FormDataModel formDataModel, int studentId, int locationId)
        {

            List<IFormFile> files = formDataModel.Files;
            string description = formDataModel.Description;

            List<string> imageLinks = new List<string>();
            string path = System.IO.Directory.GetCurrentDirectory();

            long size = files.Sum(f => f.Length);
            int check = 0;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string uniqueFileName = GetUniqueFileName(formFile.FileName);
                    var filePath = Path.Combine(path, "uploadedFiles/" + uniqueFileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    imageLinks.Add(uniqueFileName);
                    check = Map.addImageToLocation(uniqueFileName, description, studentId, locationId);
                }
            }

            return check == 1 ? Ok() : NotFound();
        }


        // POST api/<MapsController>
        [HttpPost("studentId/{studentId}/lon/{lon}/lat/{lat}/name/{name}")]
        public IActionResult Post(int studentId, double lon, double lat, string name)
        {
            return Map.newMapComponent(studentId, lon, lat, name) == 1 ? Ok() : NotFound();
        }

        // PUT api/<MapsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MapsController>/5
        [HttpDelete("locationId/{locationId}/filesUrl/{filesUrl}")]
        public IActionResult Delete(int locationId, string filesUrl)
        {
            int a = Map.deleteLocation(locationId) != 0 ? 1 : 0;

            if (a == 1)
            {
                string[] files = filesUrl.Split(',');
                foreach (string f in files)
                {
                    string file = "./uploadedFiles/" + f;
                    System.IO.File.Delete(file);
                }
                return Ok();
            }
            else
                return NotFound();
        }

        [HttpDelete("fileId/{fileId}/fileUrl/{fileUrl}")]
        public IActionResult DeleteImage(int fileId, string fileUrl)
        {

            int a = Map.deleteImage(fileId) != 0 ? 1 : 0;
            if (a == 1)
            {
                string file = "./uploadedFiles/" + fileUrl;
                System.IO.File.Delete(file);
                return Ok();
            }
            else
                return NotFound();
        }

        private string GetUniqueFileName(string fileName)
        {
            string uniqueFileName = fileName;
            int count = 1;
            string path = System.IO.Directory.GetCurrentDirectory();
            string fileExtension = Path.GetExtension(fileName);
            string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);

            while (System.IO.File.Exists(Path.Combine(path, "uploadedFiles/" + uniqueFileName)))
            {
                uniqueFileName = $"{fileNameOnly}({count++}){fileExtension}";
            }

            return uniqueFileName;
        }
    }
}
