using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersForumService.DAL.Entities;

namespace UsersForumService.DAL.Repositories.Responses
{
    public interface IResponseRepository
    {
        Task<IEnumerable<Response>> GetAllResponsesAsync();
        Task<Response?> GetResponseByIdAsync(int id);
        Task AddResponseAsync(Response response);
        Task UpdateResponseAsync(Response response);
        Task DeleteResponseAsync(int id);
        Task<IEnumerable<Response>> GetResponsesByPostIdAsync(int postId);
    }

}
