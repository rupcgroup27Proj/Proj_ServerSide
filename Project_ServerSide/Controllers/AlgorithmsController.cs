using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models.Algorithm;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlgorithmsController : ControllerBase
    {
        //Get a list of TOP 5 recommanded subject to fetch from Wikipedia
        [HttpGet("studentId/{studentId}")]
        public List<string> GetStudentRecommandations(int studentId)
        {
            return Algorithm.GetStudentRecommandations(studentId);
        }


        [HttpPut]
        public void RunAlgorithm()
        {
            Algorithm.RunAlgorithm();
        }
    }
}
