using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagerCourse.ClientTests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Services;
using Newtonsoft.Json;
using System.Net;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.ClientTests.Services.Tests
{
    [TestClass()]
    public class TasksRequestServiceTests
    {
        private readonly AuthToken token;
        private readonly TasksRequestService service;

        public TasksRequestServiceTests()
        {
            token = new UsersRequestService().GetToken("admin", "qwerty");
            service = new TasksRequestService();
        }
        [TestMethod()]
        public void GetAllTasksTest()
        {
            var tasks = service.GetAllTasks(token);
            Console.WriteLine(JsonConvert.SerializeObject(tasks));

            Assert.AreNotEqual(Array.Empty<TaskModel>(), tasks.ToArray());
        }

        [TestMethod()]
        public void GetTaskByIdTest()
        {
            var task = service.GetTaskById(token, 2);
            Console.WriteLine(JsonConvert.SerializeObject(task));
            Assert.AreNotEqual(null, task);
        }
        [TestMethod()]
        public void CreateTaskTest()
        {
            var task = new TaskModel("Task Test2", "Task for services2", DateTime.Now, DateTime.Now, "New");
            task.DeskId = 2;
            task.ExecutorId = 1;
            var result = service.CreateTask(token, task);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
        
        [TestMethod()]
        public void UpdateTaskTest()
        {
            var task = new TaskModel("Task Test2 update", "Task for services2 update", DateTime.Now, DateTime.Now.AddDays(1), "Finish");
            task.Id = 3;
            task.ExecutorId = 11;
            var result = service.UpdateTask(token, task);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteTaskTest()
        {
            var result = service.DeleteTask(token, 3);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void GetTasksByDeskTest()
        {

            var tasks = service.GetTaskByDesk(token, 2);
            Console.WriteLine(JsonConvert.SerializeObject(tasks));

            Assert.AreNotEqual(0, tasks.Count);
        }
    }
}