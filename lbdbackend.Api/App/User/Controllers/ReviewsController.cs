using lbdbackend.Service.DTOs.ReviewDTOs;
using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace lbdbackend.Api.App.User.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase {
        private readonly IReviewService _reviewService;
        public ReviewsController(IReviewService reviewService) {
            _reviewService = reviewService;
        }

        [HttpPut]
        [Route("create")]
        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> Create(ReviewCreateDTO reviewCreateDTO) {
            await _reviewService.Create(reviewCreateDTO);
            return StatusCode(201);
        }
    }
}
