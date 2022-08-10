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
        public async Task<IActionResult> GetAll() {
            return Ok(await _personService.GetPeople());
        }

        [HttpGet]
        [Route("GetByID")]
        public async Task<IActionResult> GetByID(int? id) {
            return Ok(await _personService.GetByID(id));
        }

    }

}