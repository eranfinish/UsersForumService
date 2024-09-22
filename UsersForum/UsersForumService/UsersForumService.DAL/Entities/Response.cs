using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UsersForumService.DAL.Entities
{
    public class Response
    {
        public int Id { get; set; }
        public int PostId { get; set; }  // Related post
        public int UserId { get; set; }  // User who posted the response
        public string? Name { get; set; } // Resposer Name
        public string? ResponseMessage { get; set; }

        // Navigation Properties
        [JsonIgnore] 
        public Post? Post { get; set; }  // Related post
     
        [JsonIgnore] 
        public User? User { get; set; }  // Responder user
    }
}
