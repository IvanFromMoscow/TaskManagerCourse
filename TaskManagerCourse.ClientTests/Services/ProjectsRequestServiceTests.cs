using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagerCourse.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using TaskManagerCourse.Common.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using TaskManagerCourse.Client.Models;

namespace TaskManagerCourse.Client.Services.Tests
{

    [TestClass()]
    public class ProjectsRequestServiceTests
    {
        private readonly AuthToken token; 
        private readonly ProjectsRequestService service;
        public ProjectsRequestServiceTests()
        {
            token = new UsersRequestService().GetToken("admin", "qwerty");
            service = new ProjectsRequestService();
        }
        [TestMethod()]
        public void GetAllProjectsTest()
        {       
            var projects = service.GetAllProjects(token);
            Console.WriteLine(JsonConvert.SerializeObject(projects));

            Assert.AreNotEqual(Array.Empty<ProjectModel>(), projects.ToArray());
        }

        [TestMethod()]
        public void GetProjectByIdTest()
        {        
            var project = service.GetProjectById(token, 2);
            Console.WriteLine(JsonConvert.SerializeObject(project));

            Assert.AreNotEqual(null, project);
        }
        [TestMethod()]
        public void CreateProjectTest()
        {
            var project = new ProjectModel("Megapolis", "Megapolis - aqua system", ProjectStatus.InProgress);
            project.AdminId = 1;
            var result = service.CreateProject(token, project);
            
            Assert.AreEqual(HttpStatusCode.OK, result);
        }
        [TestMethod()]
        public void UpdateProjectTest()
        {
            var project = new ProjectModel("Megapolis!!!!", "Megapolis - aqua system - important", ProjectStatus.InProgress);
            project.Id = 3;
            
            var result = service.UpdateProject(token, project);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteProjectTest()
        {
            var result = service.DeleteProject(token, 3);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void AddUsersToProjectTest()
        {
            var result = service.AddUsersToProject(token, 3, new List<int> { 5, 10, 11 });

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void RemoveUsersFromProjectTest() 
        {  
            var result = service.RemoveUsersFromProject(token, 3, new List<int> { 10, 11 });

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}