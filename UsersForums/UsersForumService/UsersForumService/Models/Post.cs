using System.Text.Json.Serialization;
using UsersForumService.DAL.Entities;

namespace UsersForumService.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        
        [JsonIgnore]
        public ICollection<int>? ResponsersIDs { get; set; }
        // Navigation Properties
        
        [JsonIgnore]
        public User? User { get; set; }  // Post creator
        
        [JsonIgnore]
        public ICollection<Response>? Responses { get; set; }  // List of responses
      
    }

    public class PostDto//Data Object
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        // Add additional fields as needed, but exclude complex ones like User and Responses
    }
}
