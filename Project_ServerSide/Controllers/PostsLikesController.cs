using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;
using System.ComponentModel.Design;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsLikesController : ControllerBase
    {
        [HttpGet("groupId/{groupId}")]
        public List<PostsLikes> Get(int groupId)
        {
            return PostsLikes.ReadPostsLikes(groupId);
        }


        [HttpPost("studentId/{studentId}/postId/{postId}")]
        public bool Post(int studentId, int postId)
        {
            return PostsLikes.InsertPostsLikes(studentId, postId);
        }


        [HttpDelete("studentId/{studentId}/postId/{postId}")]
        public IActionResult Delete(int studentId, int postId)
        {
            int num = PostsLikes.DeletePostsLikes(studentId, postId);
            if (num == 1)
                return Ok(new { message = "Resource deleted successfully" });
            else
                return NotFound(new { message = "Resource not found" });
        }
    }
}
