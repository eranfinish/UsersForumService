using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UsersForumService.DAL.Entities
{
    public class User
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string? Token { get; set; }
        public bool IsLogedIn { get; set; }
        public DateTime LastEntrance { get; set; }
        public string? Mobile { get; set; }


        // Navigation Properties
        [JsonIgnore] 
        public ICollection<Post?> Posts { get; set; }  // Posts created by user
        [JsonIgnore]
        public ICollection<Response?> Responses { get; set; }  // Responses by user

    }
}
