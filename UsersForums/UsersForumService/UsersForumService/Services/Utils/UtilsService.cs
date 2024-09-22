using dal = UsersForumService.DAL;
using UsersForumService.Models;

namespace UsersForumService.Services.Utils
{
    public class UtilsService : IUtilsService
    {
        public void User2dbUser(User user, dal.Entities.User dbUser)
        {
            dbUser.UserName = user.UserName;
            dbUser.Email = user.Email;
            dbUser.Password = user.Password;
            dbUser.IsLogedIn = user.IsLogedIn;
            dbUser.LastEntrance = DateTime.Now;
            dbUser.Mobile = user.Mobile;
            dbUser.Name = user.Name;

        }

        public void DbUser2User(dal.Entities.User dbUser, User user)
        {
            user.UserName = dbUser.UserName;
            user.Email = dbUser.Email;
            user.Password = dbUser.Password;
            user.IsLogedIn = dbUser.IsLogedIn;
            user.Name = dbUser.Name;
            user.Mobile = dbUser.Mobile;
        }

   
    }
}
