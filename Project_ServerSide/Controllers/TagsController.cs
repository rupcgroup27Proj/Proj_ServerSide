using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        // GET: api/<TagsController>
        [HttpGet("groupId/{groupId}")]
        public List<Tag> GetAllTagsByGroupId(int groupId)
        {
            return Tag.GetTags(groupId);
        }

        // GET api/<TagsController>
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

        // POST api/<TagsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TagsController>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TagsController>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
