﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Services;
using TaskManagerCourse.Common.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagerCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationContext db;
        private readonly UserService userService;
        private readonly TaskService taskService;
        public TasksController(ApplicationContext applicationContext)
        {
            db = applicationContext;
            userService = new UserService(db);
            taskService = new TaskService(db);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasksByDesk(int deskId)
        {
            var result = await taskService.GetAll(deskId).ToListAsync();
            return result == null ? NoContent() : Ok(result);
        }
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTaskForCurrentUser()
        {
            var user = userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                var result =  await taskService.GetTasksForUser(user.Id).ToListAsync();
                return result == null ? NoContent() : Ok(result);
            }
            return Unauthorized(Array.Empty<TaskModel>());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var task = taskService.Get(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaskModel taskModel)
        {
            var user = userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (taskModel != null)
                {
                    taskModel.CreatorId = user.Id;
                    bool result = taskService.Create(taskModel);
                    return result ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] TaskModel taskModel)
        {
            var user = userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (taskModel != null)
                {
                    bool result = taskService.Update(id, taskModel);
                    return result ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = taskService.Delete(id);
            return result == true ? Ok() : NotFound();
        }
    }
}
