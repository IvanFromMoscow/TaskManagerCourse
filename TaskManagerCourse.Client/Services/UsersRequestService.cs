using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using TaskManagerCourse.Client.Models;

namespace TaskManagerCourse.Client.Services
{
    public class UsersRequestService
    {
        private const string HOST = "http://localhost:5080/api/";

        private string GetDataByUrl(string url, string username, string password)
        {
            string result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            if (username != null && password != null)
            {
                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                request.Headers.Add("Authorization", "Basic " + encoded);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseStr = sr.ReadToEnd();
                    result = responseStr;
                }
            }
            return result;
        }
        public AuthToken GetToken(string username, string password)
        {
            string url = HOST + "account/token";
            string resultStr = GetDataByUrl(url, username, password);
            AuthToken token = JsonConvert.DeserializeObject<AuthToken>(resultStr);
            return token;
        }
    }
}
