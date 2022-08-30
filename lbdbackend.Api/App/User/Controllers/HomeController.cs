using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace lbdbackend.Api.App.User.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase {
        private readonly IReviewService _reviewService;
        public HomeController(IReviewService reviewService) {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var reviews = await _reviewService.GetRecentReviews();
            return Ok(new {
                reviews = reviews
            });

        }

    }
}
