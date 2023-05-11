using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        [HttpGet("groupId/{groupId}")]
        public IActionResult Get(int groupId)
        {
            Tasks tasks = new Tasks();
            List<Tasks> TasksList = tasks.ReadTaskList(groupId);

            return (TasksList.Count > 0) ? Ok(TasksList) : NotFound("No Tasks on the system");
        }


        [HttpPost]
        public bool Post([FromBody] Tasks tasks)
        {
            return tasks.InsertTasksbyTeacher();
        }



        //[HttpDelete("id/{id}")]
        //public IActionResult Delete(int id)
        //{
        //    Phones phones = new Phones();

        //    return (phones.DeletePhone(id) > 0) ? Ok("Success") : NotFound("Delete failed");
        //}
    }
}
