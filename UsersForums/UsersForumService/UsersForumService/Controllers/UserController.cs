using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsersForumService.DAL.Entities;
using UsersForumService.Models;
using UsersForumService.Services.Users;
using UsersForumService.Services.Utils;
using entities = UsersForumService.DAL.Entities;
using repos = UsersForumService.DAL.Repositories;



namespace UsersForumService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
     private readonly IUtilsService _utils;
        IUserService _userService;
        private readonly IConfiguration _config;

        public UserController( IUserService userService, IConfiguration config, IUtilsService utils)
        {      
            _userService = userService;
            _config = config;
            _utils = utils;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Models.User usr)
        {
            try
            {

             


                if (!ModelState.IsValid)
                {
                    return await Task.FromResult<IActionResult>(BadRequest(ModelState));
                }

                if (usr == null)
                {
                    return await Task.FromResult<IActionResult>(BadRequest("Invalid data."));
                }


                // scrumbling the password to prevent exposure in DB
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(usr.Password);
                usr.Password = hashedPassword;

                var res = await _userService.Register(usr);
                if (res != string.Empty)
                {
                    return await Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status401Unauthorized, res));
                }
                JwtSecurityToken token = GenerateJWT(usr);
          
                // Create user entity
                var dbUser = new entities.User();
                UserLogin userLogin = new UserLogin();
                userLogin.UserName = usr.UserName;
                userLogin.Password = usr.Password;
                dbUser = await _userService.UserLoginInfo(userLogin);
            
                return await Task.FromResult<IActionResult>(Ok(new
                {
                    User = dbUser,
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        private JwtSecurityToken GenerateJWT(Models.User usr)
        {
            usr.Name = usr.Name ?? "";
            // JWT token generation
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usr.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, usr.UserName),
                new Claim("name", usr.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),//expires in one day
                signingCredentials: creds
            );
            return token;
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLogin userLogin)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                var dbUser = new entities.User();
                //encript the password to compare it to the encripted stored password
                userLogin.Password = BCrypt.Net.BCrypt.HashPassword(userLogin.Password);
                 dbUser =  _userService.UserLoginInfo(userLogin).Result;
                
                if(dbUser == null)
                {
                    StatusCode(StatusCodes.Status400BadRequest, "One or both details maybe rong");
                }
                var usr = new Models.User();
                _utils.DbUser2User(dbUser, usr);
                JwtSecurityToken token = GenerateJWT(usr);

                return Ok(new {
                    User = dbUser,
                    token = new JwtSecurityTokenHandler().WriteToken(token) });

            }
            catch (Exception ex)
            {
                return BadRequest();
                throw;
            }

        }

        
        //[Authorize]
        [HttpGet("Logout")]
        public IActionResult Logout(Models.User user)
        {
            try
            {
                _userService.UserLogOutInfo(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }

        
    }
}
