using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UsersForumService.DAL.Entities
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
       
        
        [JsonIgnore]
        public User? User { get; set; }  // Post creator
        
    //    [JsonIgnore]
        public ICollection<Response>? Responses { get; set; }  // List of responses
    }


}
