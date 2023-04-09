﻿using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;
using Project_ServerSide.Models.SmartQuestionnaires;
using System.Text.RegularExpressions;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavListController : ControllerBase
    {
        [HttpGet("studentId/{studentId}")]
        public string Get(int studentId)
        {
            return FavList.ReadByStudentId(studentId);
        }

        [HttpPost("studentId/{studentId}/postId/{postId}")]
        public bool Post(int studentId, int postId, [FromBody] List<Tag> tags)
        {
            Questionnaire.updateStudentTags(studentId, tags);
            return FavList.Insert(studentId, postId);
        }

        [HttpPut("studentId/{studentId}/postId/{postId}")]
        public IActionResult Delete(int studentId, int postId, [FromBody] List<Tag> tags)
        {
            FavList.LowerStudentTags(studentId, tags);
             if (FavList.Delete(studentId, postId) == 1)
                return Ok(new { message = "Resource deleted successfully" });
            else
                return NotFound(new { message = "Resource not found" });
        }
    }
}
