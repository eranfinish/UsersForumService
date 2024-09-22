using UsersForumService.DAL.Entities;
using UsersForumService.DAL.Repositories.Responses;

namespace UsersForumService.Services.Responses
{
    public class ResponseService : IResponseService
    {
        private readonly IResponseRepository _responseRepository;
         public ResponseService(IResponseRepository responseRepository) {
            _responseRepository = responseRepository;
        }

        public async Task AddResponse(Response response)
        {
            await _responseRepository.AddResponseAsync(response);
        }

        public async Task DeleteResponse(int responseId)
        {
            await _responseRepository.DeleteResponseAsync(responseId);
        }

        public async Task<Response?> GetResponseByIdAsync(int id)
        {
            return await  _responseRepository.GetResponseByIdAsync(id);
        }

        public async Task<IEnumerable<Response>> GetResponsesByPostId(int postId)
        {
           return await _responseRepository.GetResponsesByPostIdAsync(postId);
        }

      

        public async Task UpdateResponse(Response response)
        {
           await _responseRepository.UpdateResponseAsync(response);
        }
    }
}
