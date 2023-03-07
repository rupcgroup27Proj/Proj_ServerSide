using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsCommentsController : ControllerBase
    {
        // GET: api/<PostsCommentsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PostsCommentsController>/5
        [HttpGet("{PostId}")]
        public List<PostsComments> Get(int PostId)
        {
            return PostsComments.ReadByPostId(PostId);
        }

        // POST api/<PostsCommentsController>
        [HttpPost]
        public int Post([FromBody] PostsComments postsComments)
        {
            return postsComments.Insert();
        }

        // PUT api/<PostsCommentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PostsCommentsController>/5
        [HttpDelete("studentId/{studentId}/commentId/{commentId}")]
        public bool Delete1(int studentId, int commentId)
        {
            return PostsComments.DeleteByStudent(studentId, commentId);
        }


        // DELETE api/<PostsCommentsController>/5
        [HttpDelete("teacherId/{teacherId}/commentId/{commentId}")]
        public int Delete2(int teacherId, int commentId)
        {
            return PostsComments.DeleteByTeacher(teacherId, commentId);
        }
    }
}
