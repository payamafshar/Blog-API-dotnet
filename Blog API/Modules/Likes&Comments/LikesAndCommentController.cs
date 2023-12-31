﻿using Blog_API.CustomController;
using Blog_API.Modules.Blog;
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
        [Authorize]
        [HttpPost]
        [Route("comment/{blogId:Guid}")]
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

        [Authorize]
        [HttpPost]
        [Route("reply/{commentId:Guid}")]

        public  async Task<ActionResult<RepyCommentEntity>> CreateReplyComment (CreateReplyCommentDto createReplyCommentDto , [FromRoute] Guid commentId )
        {
            string? email = _httpContextAccessor.HttpContext?.User.Email();

            var replyComment = await _likeAndCommentService.CreateRepyCommentAsync(createReplyCommentDto,  email, commentId);

           return Ok(replyComment);
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<BlogEntity>>> GetAllBlogs ()
        {
            var blogs = await _likeAndCommentService.GetAllBlogsAsync();

            return Ok(blogs);
        }
    }
}
