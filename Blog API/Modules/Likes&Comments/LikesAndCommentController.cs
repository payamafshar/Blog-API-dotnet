using Blog_API.CustomController;
using Blog_API.Modules.Likes_Comments.Dtos;
using Blog_API.Modules.Likes_Comments.Entities;
using Blog_API.Modules.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Modules.Likes_Comments
{
    public class LikesAndCommentController : CustomControllerBase
    {
        private readonly ILikeAndCommentService _likeAndCommentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LikesAndCommentController(ILikeAndCommentService likeAndCommentService, IHttpContextAccessor httpContextAccessor)
        {
            _likeAndCommentService = likeAndCommentService;
           _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost]
        [Route("{blogId}")]
        [Authorize]
        public async Task<ActionResult> CreateLike([FromRoute] Guid blogId)
        {
            string? email = _httpContextAccessor.HttpContext?.User.Email();

         
            var likes = await _likeAndCommentService.CreateToggleLikeAsync(blogId , email);
           
            if(likes == null)
            {
                return NotFound();
            }
             return Ok(likes);

        }
        [HttpPost]
        [Route("comment/{blogId:Guid}")]
        [Authorize]
        public async Task<ActionResult<CommentsEntity>> CreateComment(CreateCommentDto createCommentDto,[FromRoute] Guid blogId)
        {
            string? email = _httpContextAccessor.HttpContext?.User.Email();
            if(email == null)
            {
                return Unauthorized();
            }
            var comment = await _likeAndCommentService.CreateCommentAsync(createCommentDto, blogId, email);

            if(comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }
    }
}
