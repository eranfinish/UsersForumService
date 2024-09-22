using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersForumService.DAL.Entities;
using UsersForumService.DAL.Repositories.Posts;

namespace UsersForumService.DAL.Repositories.Posts
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            var posts = await _context.Posts
               .AsNoTracking()  // Prevent EF from tracking the entities
               .Include(p=>p.User)
               .ToListAsync();

            if (posts == null || !posts.Any())
            {
                throw new InvalidOperationException("No posts found.");
            }

            return posts;
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            if (_context.Posts is not null)
            {
                var post = await _context.Posts
             .Include(p => p.User)        // Include the post's author
             .ThenInclude(p => p.Responses)   // Include the responses for the post
                                              //.ThenInclude(r => r.User)    // Include the user for each response (if needed)
             .FirstOrDefaultAsync(p => p.Id == id);  // Get the post by Id

                if (post == null)
                {
                    throw new KeyNotFoundException($"Post with ID {id} not found.");
                }
                return post;
            }
            return null;
        }

        public async Task AddPostAsync(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await GetPostByIdAsync(id);
            if (post == null)
            {
                throw new KeyNotFoundException($"Post with ID {id} not found.");
            }
            // Clear the responses of this post if any exist
            if (post.Responses != null)
            {
                post.Responses.Clear();  // This clears all items in the collection without deleting them from the database
            }
            if (_context.Posts != null)
            {
                _context.Posts.Remove(post);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Post?> GetPostWithDetailsAsync(int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Responses)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                throw new KeyNotFoundException($"Post with ID {id} not found.");
            }

            return post;
        }
    }


}
