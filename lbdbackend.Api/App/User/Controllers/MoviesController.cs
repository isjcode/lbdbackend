using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace lbdbackend.Api.App.User.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase {
        private readonly IMovieService _movieService;
        private readonly IYearsService _yearsService;
        public MoviesController(IMovieService movieService, IYearsService yearsService) {
            _movieService = movieService;
            _yearsService = yearsService;
        }

        [HttpGet]
        [Route("getyears")]
        public async Task<IActionResult> GetYears() {
            return Ok(_yearsService.GetYears());
        }

        [HttpPost]
        [Route("findmovie")]
        public async Task<IActionResult> FindMovieByString(string str) {
            return Ok(await _movieService.GetByStr(str));
        }

        [HttpGet]
        [Route("getbyid")]
        public async Task<IActionResult> GetByID(int? id) {
            return Ok(await _movieService.GetByID(id));
        }

    }
}
