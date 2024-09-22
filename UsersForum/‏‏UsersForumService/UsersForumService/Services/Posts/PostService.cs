using Microsoft.EntityFrameworkCore;
using UsersForumService.DAL.Entities;
using UsersForumService.DAL.Repositories.Posts;
using UsersForumService.DAL.Repositories.Users;
using UsersForumService.Models;
using entities = UsersForumService.DAL.Entities;

namespace UsersForumService.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostService(IPostRepository postRepository, IUserRepository userRepository) {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task AddNewPostAsync(Models.Post post)
        {
            try
            {

   
            var dbPost = new entities.Post();
            var user =await _userRepository.GetUserByIdAsync(post.UserId);
            dbPost.Title = post.Title;
            dbPost.Content = post.Content;
            dbPost.User = user ?? new entities.User();
                dbPost.Name = post.Name;
            dbPost.Responses = post.Responses ?? new List<entities.Response>();

            await _postRepository.AddPostAsync(dbPost);  
            }
            catch (DbUpdateException ex)
            {
                // Log or return the detailed error message for debugging
                throw new Exception($"DB Update Error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch 
            {

                throw;
            }
        }

        public async Task UpdatePostAsync(Models.Post post)
        {
            var dbPost = new entities.Post();
            var user = await _userRepository.GetUserByIdAsync(post.UserId);
            dbPost.Title = post.Title;
            dbPost.Content = post.Content;
            dbPost.User = user ?? new entities.User();

            dbPost.Responses = post.Responses ?? new List<entities.Response>();
            await _postRepository.UpdatePostAsync(dbPost);

        }

        public async Task DeletePostAsync(int postId)
        {
            await _postRepository.DeletePostAsync(postId);
        }

        public async Task<IEnumerable<PostDto>> GetAllPosts()
        {
            var postsList = await _postRepository.GetAllPostsAsync();

            var postDtos = postsList.Select(post => new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Title = post.Title,
                Content = post.Content,
           
            }).ToList();

            return postDtos;

        }
        public async Task<entities.Post> GetPostById(int postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);

            return post;
        }
    }
}
