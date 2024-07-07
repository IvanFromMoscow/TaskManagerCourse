using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Services
{
    public class DesksRequestService : CommonRequestService
    {
        private string deskControllerUrl = HOST + "desks";

        public List<DeskModel> GetAllDesks(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, deskControllerUrl, token);
            List<DeskModel> desks = JsonConvert.DeserializeObject<List<DeskModel>>(response);
            return desks;
        }
        public DeskModel GetDeskById(AuthToken token, int deskId)
        {
            var response = GetDataByUrl(HttpMethod.Get, deskControllerUrl + $"/{deskId}", token);
            DeskModel desk = JsonConvert.DeserializeObject<DeskModel>(response);
            return desk;
        }
        public List<DeskModel> GetDesksByProject(AuthToken token, int projectId)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("projectId", projectId.ToString());
            var response = GetDataByUrlUseWebClient(HttpMethod.Get, deskControllerUrl + $"/project", token, parameters: parameters);
            List<DeskModel> desks = JsonConvert.DeserializeObject<List<DeskModel>>(response);
            return desks;
        }
        public HttpStatusCode CreateDesk(AuthToken token, DeskModel desk)
        {
            string deskJson = JsonConvert.SerializeObject(desk);
            var result = SendDataByUrl(HttpMethod.Post, deskControllerUrl, token, deskJson);
            return result;
        }
        public HttpStatusCode UpdateDesk(AuthToken token, DeskModel desk)
        {
            string deskJson = JsonConvert.SerializeObject(desk);
            var result = SendDataByUrl(HttpMethod.Patch, deskControllerUrl + $"/{desk.Id}", token, deskJson);
            return result;
        }
        public HttpStatusCode DeleteDesk(AuthToken token, int deskId)
        {
            var result = DeleteDataByUrl(deskControllerUrl + $"/{deskId}", token);
            return result;
        }
    }
}
