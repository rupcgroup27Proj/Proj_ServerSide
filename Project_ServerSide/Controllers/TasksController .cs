using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {

        [HttpGet("taskId/{taskId}")]
        public IActionResult Get(int taskId)
        {
            Tasks tasks = new Tasks();
            List<Tasks> TasksList = tasks.GetTaskByID(taskId);

            return (TasksList.Count > 0) ? Ok(TasksList) : NotFound("No Tasks on the system");
        }


        [HttpGet("groupId/{groupId}")]
        public List<Tasks> ReadTaskList(int groupId)
        {
            Tasks tasks = new Tasks();
            return tasks.ReadTaskList(groupId);

        }

        [HttpPost]
        public bool Post([FromBody] Tasks tasks)
        {
            return tasks.InsertTasksbyTeacher();
        }

    }
}
