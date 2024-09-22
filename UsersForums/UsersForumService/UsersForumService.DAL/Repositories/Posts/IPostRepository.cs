using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersForumService.DAL.Entities;

namespace UsersForumService.DAL.Repositories.Posts
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post?> GetPostByIdAsync(int id);
        Task AddPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);
        Task<Post?> GetPostWithDetailsAsync(int id);
    }

}
