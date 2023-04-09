using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavListController : ControllerBase
    {
        // GET: api/<FavListController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<FavListController>/5
        [HttpGet("studentId/{studentId}")]
        public string Get(int studentId)
        {
            return FavList.ReadByStudentId(studentId);
        }

        // POST api/<FavListController>
        [HttpPost("studentId/{studentId}/postId/{postId}")]
        public bool Post(int studentId, int postId)
        {
            return FavList.Insert(studentId, postId);
        }

        // PUT api/<FavListController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<FavListController>/5
        [HttpDelete("studentId/{studentId}/postId/{postId}")]
        public IActionResult Delete(int studentId, int postId)
        {
            FavList s = new FavList();
            int num = FavList.Delete(studentId, postId);
            if (num == 1)
                return Ok(new { message = "Resource deleted successfully" });
            else
                return NotFound(new { message = "Resource not found" });
        }



    }
}
