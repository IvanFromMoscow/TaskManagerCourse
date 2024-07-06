using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Services;
using TaskManagerCourse.Common.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagerCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesksController : ControllerBase
    {
        private readonly ApplicationContext db;
        private readonly UserService userService;
        private readonly DeskService deskService;
        public DesksController(ApplicationContext applicationContext)
        {
            db = applicationContext;
            userService = new UserService(db);
            deskService = new DeskService(db);
        }

        [HttpGet]
        public async Task<IEnumerable<CommonModel>> GetDesksForCurrentUser()
        {
            var user = userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                return await deskService.GetAll(user.Id).ToListAsync();
            }
            return Array.Empty<CommonModel>();
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var desk = deskService.Get(id);
            return desk == null ? NotFound() : Ok(desk);
        }

        [HttpGet("project")]
        public async Task<IEnumerable<CommonModel>> GetProjectDesks(int projectId)
        {
            var user = userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
               return await deskService.GetProjectDesks(projectId, user.Id).ToListAsync();
            }
            return Array.Empty<CommonModel>();
        }

        [HttpPost]
        public IActionResult Create([FromBody] DeskModel deskModel)
        {
            var user = userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (deskModel != null)
                {
                    bool result = deskService.Create(deskModel);
                    return result ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] DeskModel deskModel)
        {
            var user = userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (deskModel != null)
                {
                    bool result = deskService.Update(id, deskModel);
                    return result ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = deskService.Delete(id);
            return result ? Ok() : NotFound();
        }
    }
}
