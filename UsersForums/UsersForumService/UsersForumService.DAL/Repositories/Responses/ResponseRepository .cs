using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersForumService.DAL.Entities;

namespace UsersForumService.DAL.Repositories.Responses
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly AppDbContext _context;

        public ResponseRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Response>> GetAllResponsesAsync()
        {
            var responses = await _context.Responses.ToListAsync();
            if (responses == null || !responses.Any())
            {
                throw new InvalidOperationException("No responses found.");
            }
            return responses;
        }

        public async Task<Response?> GetResponseByIdAsync(int id)
        {
            var response = await _context.Responses.FindAsync(id);
            if (response == null)
            {
                throw new KeyNotFoundException($"Response with ID {id} not found.");
            }
            return response;
        }

        public async Task AddResponseAsync(Response response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            await _context.Responses.AddAsync(response);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateResponseAsync(Response response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            _context.Responses.Update(response);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteResponseAsync(int id)
        {
            var response = await GetResponseByIdAsync(id);
            if (response == null)
            {
                throw new KeyNotFoundException($"Response with ID {id} not found.");
            }

            _context.Responses.Remove(response);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Response>> GetResponsesByPostIdAsync(int postId)
        {
            var responses = await _context.Responses
                .Where(r => r.PostId == postId)
                .ToListAsync();

            if (responses == null || !responses.Any())
            {
                throw new InvalidOperationException($"No responses found for Post ID {postId}.");
            }

            return responses;
        }
    }

}
