using Microsoft.AspNetCore.Mvc;
using Project_ServerSide.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonesController : ControllerBase
    {
        //GET: api/<PhonesController>
        [HttpGet]
        public IActionResult Get()
        {
            Phones phones = new Phones();
            List<Phones> PhonesList = phones.Read();

            if (PhonesList.Count > 0)
            {
                return Ok(PhonesList);
            }
            else
            {
                return NotFound("No Phones on the system ");
            }
        }

        //GET: api/<PhonesController>
        [HttpGet("title/{title}")]
        public IActionResult Get(string title)// title is 'embassy'
        {
            Phones phones = new Phones();
            phones.Title = title;

            Phones res = phones.pullEmbassy();
            if (res.Title == null)
            {
                return NotFound();

            }
            else
            {
                return Ok(res);
            }
        }

        // PUT api/<PhonesController>/5
        [HttpPut("id/{id}")]
        public void Put(int id, [FromBody] Phones phones)
        {
            phones.Id = id;
            phones.Update();
        }

        // POST api/<PhonesController>/5
        [HttpPost]
        public bool Post([FromBody] Phones phones)
        {
            return phones.Insert();
        }

        //// DELETE api/<PhonesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
