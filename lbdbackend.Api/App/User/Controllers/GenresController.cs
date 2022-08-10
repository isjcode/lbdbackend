using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace lbdbackend.Api.App.User.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase {
        private readonly IGenresService _genreService;
        public GenresController(IGenresService genreService) {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            return Ok(await _genreService.GetGenres());
        }

        [HttpGet]
        [Route("GetByID")]
        public async Task<IActionResult> GetByID(int? id) {
            return Ok(await _genreService.GetByID(id));
        }

    }

}
