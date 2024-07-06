

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TaskManagerCourse.Client.Services.Tests
{
    [TestClass()]
    public class UsersRequestServiceTests
    {
        [TestMethod()]
        public void GetTokenTest()
        {
            var token = new UsersRequestService().GetToken("admin","qwerty");
            Console.WriteLine(token.access_token);
            Assert.IsNotNull(token.access_token);
        }
    }
}