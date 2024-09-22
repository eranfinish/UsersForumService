using Microsoft.AspNetCore.Identity;
using dal = UsersForumService.DAL;
using UsersForumService.Models;
using UsersForumService.DAL;
//using repos = UsersForumService.DAL.Repositories;
using UsersForumService.DAL.Repositories.Users;
using UsersForumService.Services.Utils;
namespace UsersForumService.Services.Users
{
    public class UserService : IUserService
    {
      
    //    private readonly  AppDbContext _context;
        private readonly IUserRepository _userRepository;
   private readonly IUtilsService _utils;
        public UserService(IUserRepository userRepository, IUtilsService utils) {
      
            _userRepository = userRepository;
            _utils = utils;
        }

        public async Task<string> Register(User user) 
        {
            var res = string.Empty;
            if (FindUserByUsernameAsync(user.UserName).Result)
            {
                 res = $"There is allready user with username {user.UserName}";
                return await Task.FromResult(res);
            }

            await AddUser(user);
            return await Task.FromResult(res); ;
        }

        private async Task AddUser(User user)
        {
            try
            {
                var dbUser = new dal.Entities.User();

                _utils.User2dbUser(user, dbUser);

               await _userRepository.AddUserAsync(dbUser);
               // _context.SaveChanges();
            }
            catch 
            {
                throw ;
            }
        }

        public async Task<bool> FindUserByUsernameAsync(string username)
        {
            var dbUser = await _userRepository.GetUserByUserNameAsync(username);
            return dbUser is null ? false : true;
        }

        /// <summary>
        /// Executes login for the specified user and 
        /// returns user's details for JWT token
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public async Task<dal.Entities.User?> UserLoginInfo(UserLogin userLogin)
        {
            try
            {

                var dbUser = new dal.Entities.User();
                dbUser.IsLogedIn = true;
                dbUser.LastEntrance  = DateTime.Now;

                dbUser  = await _userRepository.GetUserByUserNameAsync(userLogin.UserName);
                
                if (dbUser is null) { //if not found
                    return null;
                }
                dbUser.IsLogedIn = true;
                dbUser.LastEntrance = DateTime.Now;
                await _userRepository.UpdateUserAsync(dbUser);
                return dbUser;
            }
            catch 
            {
                throw;
            }
        }
        public async Task<bool> UserLogOutInfo(User usr)
        {
            try
            {
                var dbUser = new dal.Entities.User();
                dbUser.IsLogedIn = false;
                dbUser.LastEntrance = DateTime.Now;

                _utils.User2dbUser(usr, dbUser);
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
