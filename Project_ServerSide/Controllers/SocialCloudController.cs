using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialCloudController : ControllerBase
    {
        // GET: api/<SocialCloudController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<SocialCloudController>/
        [HttpGet("groupId/{groupId}/type/{type}")]
        public List<SocialCloud> Get(int groupId, string type)
        {
            return SocialCloud.ReadByGroupIdAndType(groupId,type);
        }

        // POST api/<SocialCloudController>
        [HttpPost("tagsJson/{tagsJson}")]
        public int Post([FromBody] SocialCloud socialCloud, string tagsJson)
        {
            return socialCloud.Insert(tagsJson);
        }


        // PUT api/<SocialCloudController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<SocialCloudController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        // DELETE api/<PostsCommentsController>/5
        [HttpDelete("postId/{postId}")]
        public int Delete(int postId)
        {
            return SocialCloud.Delete(postId);
        }
    }
}
