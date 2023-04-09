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
        [HttpGet("groupId/{groupId}")]
        public string Get(int groupId)
        {
            return SocialCloud.ReadByGroupIdAndType(groupId);
        }

        // POST api/<SocialCloudController>
        [HttpPost("tagsJson/{tagsJson}")]
        public int Post([FromBody] SocialCloud socialCloud, string tagsJson)
        {
            return socialCloud.Insert(tagsJson);
        }


        // DELETE api/<SocialCloudController>
        [HttpDelete("postId/{postId}")]
        public IActionResult Delete(int postId)
        {
            SocialCloud s = new SocialCloud();
                int num = SocialCloud.Delete(postId);
            if (num == 3)
                    return Ok();
                else
                    return NotFound();
            }
        }
    }

