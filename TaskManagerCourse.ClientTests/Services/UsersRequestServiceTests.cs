

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Net;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Services.Tests
{
    [TestClass()]
    public class UsersRequestServiceTests
    {
        [TestMethod()]
        public void GetTokenTest()
        {
            var token = new UsersRequestService().GetToken("admin", "qwerty");
            Console.WriteLine(token.access_token);
            Assert.IsNotNull(token.access_token);
        }

        [TestMethod()]
        public void CreateUser()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin", "qwerty");
            UserModel userTest = new UserModel("Ivan", "Ivanov", "ivanov@mail.ru", "123", UserStatus.User, "12232312");

            var result = service.CreateUser(token, userTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
            Assert.IsNotNull(token.access_token);
        }

        [TestMethod()]
        public void GetAllUsersTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin", "qwerty");

            var result = service.GetAllUsers(token);

            Console.WriteLine(result.Count);
            Assert.AreNotEqual(Array.Empty<UserModel>(), result.ToArray());
            Assert.IsNotNull(token.access_token);
        }

        [TestMethod()]
        public void DeleteUserTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin", "qwerty");

            var result = service.DeleteUser(token, 6);
            service.DeleteUser(token, 7);
            service.DeleteUser(token, 8);
            service.DeleteUser(token, 9);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void CreateMultipleUserTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin", "qwerty");
            var users = new List<UserModel>()
            {
                new UserModel("Anny", "Bond", "bond@mail.ru", "123", UserStatus.User, "8678678678"),
                new UserModel("James", "Bond", "bondiana@mail.ru", "123", UserStatus.Editor, "343456546"),
            };

            var result = service.CreateMultipleUser(token, users);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
        [TestMethod()]
        public void UpdateUser()
        {
            UserModel userTest = new UserModel("Ivan", "Ivanov", "i@mail.ru", "999", UserStatus.Editor, "9999999");
            var service = new UsersRequestService();
            var token = service.GetToken("admin", "qwerty");
            userTest.Id = 5;
            var result = service.UpdateUser(token, userTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}