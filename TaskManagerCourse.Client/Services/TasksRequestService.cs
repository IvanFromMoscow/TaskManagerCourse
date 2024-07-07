using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Services;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.ClientTests.Services
{
    public class TasksRequestService : CommonRequestService
    {
        private string taskControllerUrl = HOST + "tasks";
        public List<TaskModel> GetAllTasks(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, taskControllerUrl + "/user", token);
            List<TaskModel> tasks = JsonConvert.DeserializeObject<List<TaskModel>>(response);
            return tasks;
        }
        public TaskModel GetTaskById(AuthToken token, int taskId)
        {
            var response = GetDataByUrl(HttpMethod.Get, taskControllerUrl + $"/{taskId}", token);
            TaskModel task = JsonConvert.DeserializeObject<TaskModel>(response);
            return task;
        }
        public List<TaskModel> GetTaskByDesk(AuthToken token, int deskId)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("deskId", deskId.ToString());
            var response = GetDataByUrlUseWebClient(HttpMethod.Get, taskControllerUrl, token, parameters: parameters);
            List<TaskModel> tasks = JsonConvert.DeserializeObject<List<TaskModel>>(response);
            return tasks;
        }
        public HttpStatusCode CreateTask(AuthToken token, TaskModel task)
        {
            string taskJson = JsonConvert.SerializeObject(task);
            var result = SendDataByUrl(HttpMethod.Post, taskControllerUrl, token, taskJson);
            return result;
        }
        public HttpStatusCode UpdateTask(AuthToken token, TaskModel task)
        {
            string taskJson = JsonConvert.SerializeObject(task);
            var result = SendDataByUrl(HttpMethod.Patch, taskControllerUrl + $"/{task.Id}", token, taskJson);
            return result;
        }
        public HttpStatusCode DeleteTask(AuthToken token, int taskId)
        {
            var result = DeleteDataByUrl(taskControllerUrl + $"/{taskId}", token);
            return result;
        }
    }
}
