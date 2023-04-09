using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsCommentsController : ControllerBase
    {
        [HttpGet("postId/{postId}")]
        public List<PostsComments> Get(int postId)
        {
            return PostsComments.ReadByPostId(postId);
        }


        [HttpPost]
        public bool Post([FromBody] PostsComments postsComments)
        {
            return postsComments.Insert();
        }


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

