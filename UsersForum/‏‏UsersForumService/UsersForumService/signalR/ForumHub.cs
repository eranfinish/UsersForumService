using Microsoft.AspNetCore.SignalR;

namespace UsersForumService.signalR
{
    public class ForumHub:Hub
    {
        // This method broadcasts a new post to all clients
        public async Task SendNewPost(int postId, int userId, string title, string content, string name)
        {
            try
            {
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                {
                    throw new ArgumentException("Title or content cannot be empty");
                }

                 await Clients.All.SendAsync("ReceiveNewPost", postId, userId, title, content, name);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in SendNewPost: {ex.Message}");
                throw;  // Rethrow the error so the client gets the exception
            }
        }

        // This method broadcasts a new response to all clients
        public async Task SendNewResponse(int postId, int userId, string name, string responseMessage)
        {
            try
            {


                await Clients.All.SendAsync("ReceiveNewResponse", postId, userId, name, responseMessage);
            }
            catch (Exception ex)
            {

                // Log the error
                Console.WriteLine($"Error in SendNewResponse: {ex.Message}");
                throw;  // Rethrow the error so the client gets the exception
            }
        }
    }
}
