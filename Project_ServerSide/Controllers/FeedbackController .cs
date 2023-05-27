using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        [HttpGet("groupId/{groupId}")]
        public List<Feedback> GetFeedbackList(int groupId)
        {
            Feedback feedback = new Feedback();
            return feedback.GetFeedbackList(groupId);

        }


        [HttpPost]
        public bool Post([FromBody] Feedback feedback)
        {
            return feedback.Insert();
        }


      
    }
}
