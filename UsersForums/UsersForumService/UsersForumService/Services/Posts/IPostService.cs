using entities = UsersForumService.DAL.Entities;
using UsersForumService.Models;

namespace UsersForumService.Services.Posts
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPosts();
        Task AddNewPostAsync(Models.Post post);
        Task DeletePostAsync(int postId);
        Task UpdatePostAsync(Models.Post post);
        Task<entities.Post> GetPostById(int postId);


    }
}
