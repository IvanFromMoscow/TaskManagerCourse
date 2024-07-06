using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Services;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        public readonly ApplicationContext db;
        private readonly UserService userService;
        private readonly ProjectService projectService;
        public ProjectsController(ApplicationContext applicationContext)
        {
            db = applicationContext;
            userService = new UserService(db);
            projectService = new ProjectService(db);
        }
        [HttpGet]
        public async Task<IEnumerable<CommonModel>> Get()
        {
            var user = userService.GetUser(HttpContext.User.Identity.Name);
            if (user?.Status == UserStatus.Admin)
            {
                return await projectService.GetAll().ToListAsync();
            }
            else
            {
                return await projectService.GetByUserId(user.Id);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var project = projectService.Get(id);
            return project == null ? NoContent() : Ok(project);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectModel projectModel)
        {
            if (projectModel != null)
            {
                var user = userService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        var admin = db.ProjectAdmins.FirstOrDefault(a => a.UserId == user.Id);
                        if (admin == null)
                        {
                            admin = new ProjectAdmin(user);
                            db.ProjectAdmins.Add(admin);
                            db.SaveChanges();
                        }
                        projectModel.AdminId = admin.Id;
                        bool result = projectService.Create(projectModel);
                        return result ? Ok() : NotFound();
                    }
                }
                return Unauthorized();
            }
            return BadRequest();
        }
        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] ProjectModel projectModel)
        {
            if (projectModel != null)
            {
                var user = userService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        bool result = projectService.Update(id, projectModel);
                        return result ? Ok() : NotFound();
                    }
                    return Unauthorized();
                }
            }
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = projectService.Delete(id);
            return result ? Ok() : NotFound();
        }

        [HttpPatch("{id}/users")]
        public IActionResult AddUsersToProject(int id, [FromBody] List<int> userIds)
        {
            if (userIds != null)
            {
                var user = userService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        projectService.AddUsersToProject(id, userIds);
                        return Ok();
                    }
                    return Unauthorized();
                }
            }
            return BadRequest();
        }
        [HttpPatch("{id}/users/remove")]
        public IActionResult RemoveUsersFromProject(int id, [FromBody] List<int> userIds)
        {
            if (userIds != null)
            {
                var user = userService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        bool result = projectService.RemoveUsersFromProject(id, userIds);
                        return result ? Ok() : NoContent();
                    }
                    return Unauthorized();
                }
            }
            return BadRequest();
        }
    }

}
