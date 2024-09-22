using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersForumService.Models;
using UsersForumService.Services.Posts;

namespace UsersForumService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize]
        [HttpGet("getAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                var posts = await _postService.GetAllPosts();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            
        }
        [Authorize]
        [HttpGet("getPostById/{id}")]
        public async Task<IActionResult> GetPostById(int id)
        { 
         try
            {
                var posts = await _postService.GetPostById(id);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
        [Authorize]
        [HttpPost("addNewPost")]
        public async Task<IActionResult> AddNewPost(Models.Post post)
        {
            try
            {
               await _postService.AddNewPostAsync(post);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("deletePost/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                await _postService.DeletePostAsync(postId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);

            }
        }

        [Authorize]
        [HttpPost("editPost")]
        public IActionResult EditPost(Models.Post post)
        {
            try
            {
                _postService.UpdatePostAsync(post);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}
