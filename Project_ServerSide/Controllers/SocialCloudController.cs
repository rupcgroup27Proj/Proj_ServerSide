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
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SocialCloudController>/5
        [HttpGet("{groupId}")]
        public List<SocialCloud> Get(int groupId)
        {
            return SocialCloud.ReadBygroupId(groupId);
        }

        // POST api/<SocialCloudController>
        [HttpPost]
        public int Post([FromBody] SocialCloud socialCloud)
        {
            return socialCloud.Insert();
        }

        // POST api/<SocialCloudController>
        [HttpPost("{postId}/{tagId}")]
        public int PostTagsToPost(int postId, int tagId)
        {
            SocialCloud s = new SocialCloud();
            return s.InsertTagsToPost(postId, tagId);
        }

        // PUT api/<SocialCloudController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SocialCloudController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // DELETE api/<PostsCommentsController>/5
        [HttpDelete("studentId/{studentId}/postId/{postId}")]
        public bool Delete1(int studentId, int postId)
        {
            return SocialCloud.DeleteByStudent(studentId, postId);
        }


        // DELETE api/<PostsCommentsController>/5
        [HttpDelete("teacherId/{teacherId}/postId/{postId}")]
        public int Delete2(int teacherId, int postId)
        {
            return SocialCloud.DeleteByTeacher(teacherId, postId);
        }
    }
}
