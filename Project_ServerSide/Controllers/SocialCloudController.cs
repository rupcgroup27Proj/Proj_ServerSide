﻿using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialCloudController : ControllerBase
    {
        [HttpGet("groupId/{groupId}")]
        public string Get(int groupId)
        {
            return SocialCloud.ReadSocialCloudByGroupId(groupId);
        }


        [HttpPost("tagsJson/{tagsJson}")]
        public IActionResult Post([FromBody] SocialCloud socialCloud, string tagsJson)
        {
            return socialCloud.InsertSocialCloud(tagsJson) != 0 ? Ok() : NotFound();
        }


        [HttpDelete("postId/{postId}")]
        public IActionResult Delete(int postId)
        {
            return SocialCloud.DeleteFromSocialCloud(postId) != 0 ? Ok() : NotFound();
        }
    }
}

