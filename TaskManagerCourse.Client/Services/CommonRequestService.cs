using DryIoc;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using TaskManagerCourse.Client.Models;

namespace TaskManagerCourse.Client.Services
{
    public abstract class CommonRequestService
    {
        public const string HOST = "http://localhost:5080/api/";
        protected string GetDataByUrl(HttpMethod method, string url, AuthToken token, string username = null, string password = null)
        {
            string result = string.Empty;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method.Method;

            if (username != null && password != null)
            {
                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                request.Headers.Add("Authorization", "Basic " + encoded);
            }
            else if (token != null)
            {
                request.Headers.Add("Authorization", "Bearer " + token.access_token);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string responseStr = sr.ReadToEnd();
                result = responseStr;
            }
            return result;
        }

        protected string GetDataByUrlUseWebClient(HttpMethod method, string url, AuthToken token, string username = null, string password = null, Dictionary<string, string> parameters = null)
        {
            WebClient client = new WebClient();
            if (username != null && password != null)
            {
                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                client.Headers.Add("Authorization", "Basic " + encoded);
            }
            else if (token != null)
            {
                client.Headers.Add("Authorization", "Bearer " + token.access_token);
            }
            if (parameters != null)
            {
                foreach (var key in parameters.Keys)
                {
                    client.QueryString.Add(key, parameters[key]);
                }
            }
            byte[]? data = Array.Empty<byte>();
            if (method == HttpMethod.Post)
            {
                data = client.UploadValues(url, method.Method, client.QueryString);
            }
            if (method == HttpMethod.Get)
            {
                data = client.DownloadData(url);
            }
            return Encoding.UTF8.GetString(data);
        }
        protected HttpStatusCode SendDataByUrl(HttpMethod method, string url, AuthToken token, string data)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            if (method == HttpMethod.Post)
            {
                result = client.PostAsync(url, content).Result;
            }
            if (method == HttpMethod.Patch)
            {
                result = client.PatchAsync(url, content).Result;
            }

            return result.StatusCode;
        }
        protected HttpStatusCode DeleteDataByUrl(string url, AuthToken token)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);
            result = client.DeleteAsync(url).Result;
            return result.StatusCode;
        }
    }
}
