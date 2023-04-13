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
            return Tag.GetAllTagsByGroupId(groupId);
        }


        [HttpGet("builtInTags")]
        public List<Tag> GetBuiltInTags()
        {
            return Tag.GetBuiltInTags();
        }
    }
}
