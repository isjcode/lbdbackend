using lbdbackend.Service.DTOs.CommentDTOs;
using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace lbdbackend.Api.App.User.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService) {
            _commentService = commentService;   
        }
        [HttpPut]
        [Route("createcomment")]
        [Authorize(Roles = "Member")]

        public async Task<IActionResult> CreateComment(CommentCreateDTO createCommentDTO) {
            await _commentService.CreateComment(createCommentDTO);
            return StatusCode(201);
        }
    }
}
