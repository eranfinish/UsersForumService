namespace UsersForumService.Models
{
    public class User : UserLogin
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }

        public string? Token { get; set; }
        public bool IsLogedIn { get; set; }
        public DateTime? LastEntrance { get; set; }
        public string? Mobile { get; set; }
    }

    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}
