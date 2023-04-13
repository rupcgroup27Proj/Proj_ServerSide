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
            return PostsComments.ReadpostsComments(postId);
        }


        [HttpPost]
        public bool Post([FromBody] PostsComments postsComments)
        {
            return postsComments.InsertPostsComments();
        }


        [HttpDelete("commentId/{commentId}")]
        public IActionResult Delete(int commentId)
        {
            PostsComments s = new PostsComments();
            int num = PostsComments.DeletePostsComments(commentId);
            if (num == 1)
                return Ok(new { message = "Resource deleted successfully" });
            else
                return NotFound(new { message = "Resource not found" });
        }
    }
}

