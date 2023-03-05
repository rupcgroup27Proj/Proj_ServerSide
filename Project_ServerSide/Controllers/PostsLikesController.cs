using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsLikesController : ControllerBase
    {
        // GET: api/<PostsLikesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PostsLikesController>/5
        [HttpGet("{StudentId}")]
        public List<PostsLikes> Get(int studentId)
        {
            return PostsLikes.ReadByStudentId(studentId);
        }

        // POST api/<PostsLikesController>
        [HttpPost]
        public bool Post([FromBody] int studentId, int postId)
        {
            return PostsLikes.Insert(studentId, postId);
        }

        // PUT api/<PostsLikesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PostsLikesController>/5
        [HttpDelete("studentId/{studentId}/postId/{postId}")]
        public IActionResult Delete(int studentId, int postId)
        {
            PostsLikes f = new PostsLikes();
            int num = f.Delete(studentId, postId);
            if (num == 1)
                return Ok();
            else
                return NotFound("id " + postId.ToString() + " was not found");
        }
    }
}
