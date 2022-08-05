using lbdbackend.Service.DTOs.PersonDTOs;
using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace lbdbackend.Api.App.Admin.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase {
        private readonly IPersonService _personService;
        public PeopleController(IPersonService personService) {
            _personService = personService;
        }

        [HttpPut]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] PersonCreateDTO personCreateDTO) {
            await _personService.Create(personCreateDTO);
            return StatusCode(201);
        }
        [HttpPost]
        [Route("DeleteOrRestore")]
        public async Task<IActionResult> DeleteOrRestore(int? id) {
            await _personService.DeleteOrRestore(id);
            return StatusCode(201);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(int? id, [FromForm] PersonUpdateDTO personUpdateDTO) {
            await _personService.Update(id, personUpdateDTO);
            return StatusCode(201);
        }

    }
}
