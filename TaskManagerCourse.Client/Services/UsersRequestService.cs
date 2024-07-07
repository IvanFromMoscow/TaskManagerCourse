using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Services
{
    public class UsersRequestService : CommonRequestService
    {
       
        private string userControllerUrl = HOST + "users";

        public AuthToken GetToken(string username, string password)
        {
            string url = HOST + "account/token";
            string resultStr = GetDataByUrl(HttpMethod.Post, url, null, username, password);
            AuthToken token = JsonConvert.DeserializeObject<AuthToken>(resultStr);
            return token;
        }

        public HttpStatusCode CreateUser(AuthToken token, UserModel user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = SendDataByUrl(HttpMethod.Post, userControllerUrl + "/create", token, userJson);
            return result;
        }
        public List<UserModel> GetAllUsers(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, userControllerUrl, token);
            List<UserModel> users = JsonConvert.DeserializeObject<List<UserModel>>(response);
            return users;
        }
        public UserModel GetCurrentUser(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, HOST + "account/info", token);
            UserModel user = JsonConvert.DeserializeObject<UserModel>(response);
            return user;
        }
        public HttpStatusCode DeleteUser(AuthToken token, int userId)
        {
            var result = DeleteDataByUrl(userControllerUrl + $"/delete/{userId}", token);
            return result;
        }
        public HttpStatusCode CreateMultipleUser(AuthToken token, List<UserModel> users)
        {
            string userJson = JsonConvert.SerializeObject(users);
            var result = SendDataByUrl(HttpMethod.Post, userControllerUrl + "/create/all", token, userJson);
            return result;
        }

        public HttpStatusCode UpdateUser(AuthToken token, UserModel user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = SendDataByUrl(HttpMethod.Patch, userControllerUrl + $"/update/{user.Id}", token, userJson);
            return result;
        }
    }
}
