using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersForumService.DAL.Entities;
using UsersForumService.Services.Responses;

namespace UsersForumService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private readonly IResponseService _responseService;
        public ResponseController(IResponseService responseService)
        {
            _responseService = responseService;
        }


        [Authorize]
        [HttpGet("getAllResponses/{postId}")]
        public async Task<IActionResult> GetAllResponses(int postId)
        {
            try
            {
                await _responseService.GetResponsesByPostId(postId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);

            }

        }

        [Authorize]
        [HttpPost("addNewResponse")]
        public async Task<IActionResult> AddNewResponse(Response response)
        {
            try
            {
               await _responseService.AddResponse(response);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);

            }
        }

        [Authorize]
        [HttpPost("editResponse")]
        public async Task< IActionResult> EditResponse(Response response)
        {
            try
            {
                await _responseService.UpdateResponse(response);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);

            }

        }
        
        [Authorize]
        [HttpPost("deleteResponse")]
        public async Task<IActionResult> DeleteResponse(int responseId)
        {
            try
            {
                await _responseService.DeleteResponse(responseId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);

            }

        }
    }

}

