using Microsoft.AspNetCore.Identity;
using UsersForumService.Models;
using dal = UsersForumService.DAL;

namespace UsersForumService.Services.Users
{
    public interface IUserService
    {
        Task<bool> FindUserByUsernameAsync(string usrname);

       // void AddUser(User user);
        Task<dal.Entities.User?> UserLoginInfo(UserLogin userLogin);   
        Task<bool> UserLogOutInfo(User usr);
        Task<string> Register(User user);


    }
}
