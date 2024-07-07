using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagerCourse.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Client.Models;
using Newtonsoft.Json;
using System.Net;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Services.Tests
{
    [TestClass()]
    public class DesksRequestServiceTests
    {
        private readonly AuthToken token;
        private readonly DesksRequestService service;

        public DesksRequestServiceTests()
        {
            token = new UsersRequestService().GetToken("admin", "qwerty");
            service = new DesksRequestService();
        }
        [TestMethod()]
        public void GetAllDesksTest()
        {
            var desks = service.GetAllDesks(token);
            Console.WriteLine(JsonConvert.SerializeObject(desks));

            Assert.AreNotEqual(Array.Empty<DeskModel>(), desks.ToArray());
        }

        [TestMethod()]
        public void GetDeskByIdTest()
        {
            var desk = service.GetDeskById(token, 2);
            Assert.AreNotEqual(null, desk);
        }
        [TestMethod()]
        public void CreateDeskTest()
        {
            var desk = new DeskModel("Desk Test2", "Desk for services2", true, new string[] { "New", "Fin"});
            desk.AdminId = 1;
            desk.ProjectId = 2;
            var result = service.CreateDesk(token, desk);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
        [TestMethod()]
        public void UpdateDeskTest()
        {
            var desk = new DeskModel("Desk Test update", "Desk update for services", false, new string[] { "New", "Finish" });
            desk.Id = 2;
            desk.AdminId = 1;
            desk.ProjectId = 2;

            var result = service.UpdateDesk(token, desk);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteDeskTest()
        {
            var result = service.DeleteDesk(token, 5);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void GetDesksByProjectTest()
        {

            var desk = service.GetDesksByProject(token, 2);
            Console.WriteLine(JsonConvert.SerializeObject(desk));

            Assert.AreNotEqual(null, desk);
        }
    }
}