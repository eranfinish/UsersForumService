using Microsoft.AspNetCore.Identity;
using UsersForumService.Models;
namespace UsersForumService.Services.Users
{
    public interface IUserService
    {
        Task<bool> FindUserByEmailAsync(string email);

       // void AddUser(User user);
        Task<bool> UserLoginInfo(UserLogin userLogin);   
        Task<bool> UserLogOutInfo(User usr);
        Task<string> Register(User user);
    }
}
