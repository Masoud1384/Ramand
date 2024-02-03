using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RamandAPI.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get(int id)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("getpr")]

        public string Get()
        {
            return "value";
        }
    }
}
