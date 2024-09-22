using dal = UsersForumService.DAL;
using UsersForumService.Models;

namespace UsersForumService.Services.Utils
{
    public interface IUtilsService
    {
        void DbUser2User(dal.Entities.User dbUser, User user);
        void User2dbUser(User user, dal.Entities.User dbUser);



    }
}
