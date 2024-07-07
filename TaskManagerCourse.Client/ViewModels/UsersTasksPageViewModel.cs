using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Services;
using TaskManagerCourse.ClientTests.Services;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.ViewModels
{
    public class UsersTasksPageViewModel : BindableBase
    {
        private AuthToken token;
        private TasksRequestService tasksRequestService;
        private UsersRequestService usersRequestService;
        public UsersTasksPageViewModel(AuthToken token)
        {
            this.token = token;
            tasksRequestService = new TasksRequestService();
            usersRequestService = new UsersRequestService();
        }

        public List<TaskClient> AllTasks
        {
            get => tasksRequestService.GetAllTasks(token).Select(t => 
            
            {
                var taskClient = new TaskClient(t);
                taskClient.Creator = usersRequestService.GetUserById(token, t.CreatorId ?? 0);
                taskClient.Executor = usersRequestService.GetUserById(token, t.ExecutorId ?? 0);
                return taskClient;
            }).ToList();
        }
    }
}
