using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Services;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext db;
        private readonly UserService userService;
        public AccountController(ApplicationContext applicationContext)
        {
            db = applicationContext;
            userService = new UserService(db);
        }

        [HttpGet("info")]
        public IActionResult GetUserInfo() 
        {
            string username = HttpContext.User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.Email == username);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpPost("token")]
        public IActionResult GetToken()
        {
            var userData = userService.GetUserLoginPassFromBasicAuth(Request);
            var login = userData.Item1;
            var password = userData.Item2;
            var identity = userService.GetIdentity(login, password);

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name,
            };
            return Ok(response);
        }
        [Authorize]
        [HttpPatch("update")]
        public IActionResult UpdateUser([FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                string username = HttpContext.User.Identity.Name;
                User userForUpdate = db.Users.FirstOrDefault(x => x.Email == username);
                if (userForUpdate != null)
                {
                    userForUpdate.FirstName = userModel.FirstName;
                    userForUpdate.LastName = userModel.LastName;
                    userForUpdate.Password = userModel.Password;
                    userForUpdate.Phone = userModel.Phone;
                    userForUpdate.Photo = userModel.Photo;

                    db.Users.Update(userForUpdate);
                    db.SaveChanges();
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

    }
}
