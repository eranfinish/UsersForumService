using Microsoft.AspNetCore.Identity;
using dal = UsersForumService.DAL;
using UsersForumService.Models;
using UsersForumService.DAL;
using repos = UsersForumService.DAL.Repositories;
using UsersForumService.DAL.Repositories.Users;
namespace UsersForumService.Services.Users
{
    public class UserService : IUserService
    {
      
    //    private readonly  AppDbContext _context;
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) {
      
            _userRepository = userRepository;
        }

        public Task<string> Register(User user) 
        {
            var res = string.Empty;
            if (FindUserByEmailAsync(user.Email).Result)
            {
                 res = $"There is allready user with email {user.Email}";
                return Task.FromResult(res);
            }

            AddUser(user);
            return Task.FromResult(res); ;
        }

        private void AddUser(User user)
        {
            try
            {
                var dbUser = new dal.Entities.User();

                user2dbUser(user, dbUser);

                _userRepository.AddUserAsync(dbUser);
               // _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        private static void user2dbUser(User user, dal.Entities.User dbUser)
        {
            dbUser.UserName = user.UserName;
            dbUser.Email = user.Email;
            dbUser.Password = user.Password;
            dbUser.IsLogedIn = user.IsLogedIn;
            dbUser.LastEntrance = DateTime.Now;
            dbUser.Mobile = user.Mobile;
        }

        private static void dbUser2User(User user, dal.Entities.User dbUser)
        {
              user.UserName=dbUser.UserName;
            user.Email=dbUser.Email;
            user.Password=dbUser.Password;
            user.IsLogedIn=dbUser.IsLogedIn;
           
            user.Mobile = dbUser.Mobile;
        }
        public async Task<bool> FindUserByEmailAsync(string email)
        {
            var dbUser = await _userRepository.FindUserByEmailAsync(email);
            return dbUser is null ? false : true;
        }

        public async Task<bool> UserLoginInfo(UserLogin userLogin)
        {
            try
            {

                var dbUser = new dal.Entities.User();
                dbUser.IsLogedIn = true;
                dbUser.LastEntrance  = DateTime.Now;

                dbUser  = await _userRepository.GetUserByUserNameAsync(userLogin.Password);
                
                if (dbUser is null) { 
                    return false;
                }
                dbUser.IsLogedIn = true;
                dbUser.LastEntrance = DateTime.Now;
                await _userRepository.UpdateUserAsync(dbUser);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public async Task<bool> UserLogOutInfo(User usr)
        {
            try
            {
                var dbUser = new dal.Entities.User();
                dbUser.IsLogedIn = false;
                dbUser.LastEntrance = DateTime.Now;

                user2dbUser(usr, dbUser);
                await _userRepository.UpdateUserAsync(dbUser);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

      

     
    }
}
