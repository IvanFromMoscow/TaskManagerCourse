using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly ApplicationContext db;
        public UsersController(ApplicationContext applicationContext)
        {
            db = applicationContext;
        }
        
        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok("Hello world!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                User newUser = new User(
                    userModel.FirstName, userModel.LastName,
                    userModel.Email,
                    userModel.Password,
                    userModel.Status,
                    userModel.Phone,
                    userModel.Photo);
                db.Users.Add(newUser);
                db.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                User userForUpdate = db.Users.FirstOrDefault(x => x.Id == id);
                if (userForUpdate != null)
                {
                    userForUpdate.FirstName = userModel.FirstName;
                    userForUpdate.LastName = userModel.LastName;
                    userForUpdate.Email = userModel.Email;
                    userForUpdate.Password = userModel.Password;
                    userForUpdate.Status = userModel.Status;
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
                User userForDelete = db.Users.FirstOrDefault(x => x.Id == id);
                if (userForDelete != null)
                {
                    db.Users.Remove(userForDelete);
                    db.SaveChanges();
                    return Ok();
                }
                return NotFound();
        }

        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetUsers() 
        {
            return await db.Users.Select(u => u.ToDto()).ToListAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create/all")]
        public async Task<IActionResult> CreateMultipleUsers([FromBody] List<UserModel> userModels)
        {
            if (userModels != null && userModels.Count > 0)
            {
                var newUsers = userModels.Select(u => 
                new User(u)).ToList();
                db.Users.AddRange(newUsers);
                await db.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
    }


    
}
