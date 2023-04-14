using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;


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
            List<Phones> PhonesList = phones.ReadPhoneList(groupId);

            return (PhonesList.Count > 0) ? Ok(PhonesList) : NotFound("No Phones on the system");
        }


        [HttpPost]
        public bool Post([FromBody] Phones phones)
        {
            return phones.InsertPhone();
        }


        [HttpDelete("id/{id}")]
        public IActionResult Delete(int id)
        {
            Phones phones = new Phones();

            return (phones.DeletePhone(id) > 0) ? Ok("Success") : NotFound("Delete failed");
        }
    }
}
