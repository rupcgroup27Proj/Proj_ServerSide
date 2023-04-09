using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;
using System.Text.RegularExpressions;


namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonesController : ControllerBase
    {
        [HttpGet("groupId/{groupId}")]
        public IActionResult Get(int groupId)
        {
            Phones phones = new Phones();
            List<Phones> PhonesList = phones.Read(groupId);

            return (PhonesList.Count > 0) ? Ok(PhonesList) : NotFound("No Phones on the system");
        }

        [HttpGet("title/{title}")]
        public IActionResult Get(string title)// title is 'embassy'
        {
            Phones phones = new Phones();
            phones.Title = title;
            Phones res = phones.pullEmbassy();

            return (res.Title == null) ? NotFound() : Ok(res);
        }

        [HttpPut("id/{id}")]
        public IActionResult Put(int id, [FromBody] Phones phones)
        {
            phones.Id = id;

            return (phones.Update() == 1) ? Ok(phones) : NotFound();
        }

        [HttpPost]
        public bool Post([FromBody] Phones phones)
        {
            return phones.Insert();
        }

        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            Phones phones = new Phones();

            return (phones.Delete(id) > 0) ? Ok("Success") : NotFound("Delete failed");
        }
    }
}
