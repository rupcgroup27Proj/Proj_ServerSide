using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsCommentsController : ControllerBase
    {
        // GET: api/<PostsCommentsController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<PostsCommentsController>/5
        [HttpGet("postId/{postId}")]
        public List<PostsComments> Get(int postId)
        {
            return PostsComments.ReadByPostId(postId);
        }

        // POST api/<PostsCommentsController>
        [HttpPost]
        public bool Post([FromBody] PostsComments postsComments)
        {
            return postsComments.Insert();

        }

        // PUT api/<PostsCommentsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        [HttpDelete("commentId/{commentId}")]
        public IActionResult Delete(int commentId)
        {
            PostsComments s = new PostsComments();
            int num = PostsComments.Delete(commentId);
            if (num == 1)
                return Ok(new { message = "Resource deleted successfully" });
            else
                return NotFound(new { message = "Resource not found" });
        }

    }
}

