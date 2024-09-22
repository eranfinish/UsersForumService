using UsersForumService.DAL.Entities;

namespace UsersForumService.Services.Responses
{
    public interface IResponseService
    {
        Task<IEnumerable<Response>> GetResponsesByPostId(int postId); ///
        Task AddResponse(Response response);
        Task UpdateResponse(Response response);
        Task DeleteResponse(int responseId);
        Task<Response?> GetResponseByIdAsync(int id);
    }
   
}
