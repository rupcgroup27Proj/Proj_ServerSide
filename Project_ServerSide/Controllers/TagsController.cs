using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        [HttpGet("groupId/{groupId}")]
        public List<Tag> GetAllTagsByGroupId(int groupId)
        {
            return Tag.GetTags(groupId);
        }


        [HttpGet("postId/{postId}")]
        public List<Tag> GetTagsByPostId(int postId)
        {
            return Tag.ReadTagInPost(postId);
        }


        [HttpGet("builtInTags")]
        public List<Tag> GetBuiltInTags()
        {
            return Tag.GetBuiltInTags();
        }
    }
}
