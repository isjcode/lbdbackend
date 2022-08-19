using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace lbdbackend.Api.App.User.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase {
        private readonly IPersonService _personService;

        public PeopleController(IPersonService personService) {
            _personService = personService;
        }
        
        [HttpGet]
        [Route("getmoviepeople")]
        public async Task<IActionResult> GetMoviePeople(int? id) {
            var people = await _personService.GetMoviePeople(id);
            return Ok(people);
        }
    }
}
