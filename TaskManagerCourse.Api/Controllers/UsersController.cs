using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Services;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly ApplicationContext db;
        private readonly UserService userService;
        public UsersController(ApplicationContext applicationContext)
        {
            db = applicationContext;
            userService = new UserService(db);
        }
        
        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok($"Сервер запущен. Время запуска { DateTime.Now }");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                bool result = userService.Create(userModel);
                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                bool result = userService.Update(id, userModel);
                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            bool result = userService.Delete(id);
            return result ? Ok() : NotFound();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = userService.Get(id);
            return user == null ? NoContent() : Ok(user);
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
                bool result = userService.CreateMultipleUsers(userModels);
                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }
    }


    
}
