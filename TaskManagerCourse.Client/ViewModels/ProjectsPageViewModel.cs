using Prism.Mvvm;
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
    public class ProjectsPageViewModel : BindableBase
    {
        #region Commands
        public DelegateCommand OpenNewProjectCommand;
        public DelegateCommand<object> ShowProjectInfoCommand;
        public DelegateCommand<object> OpenUpdateProjectCommand;
        #endregion
        private AuthToken token;
        private UsersRequestService usersRequestService;
        private ProjectsRequestService projectsRequestService;
        private CommonViewService commonViewService;
        public ProjectsPageViewModel(AuthToken token = null)
        {
            this.token = token;
            usersRequestService = new UsersRequestService();
            commonViewService = new CommonViewService();
            projectsRequestService = new ProjectsRequestService();

            OpenNewProjectCommand = new DelegateCommand(OpenNewProject);
            ShowProjectInfoCommand = new DelegateCommand<object>(ShowProjectInfo);
            OpenUpdateProjectCommand = new DelegateCommand<object>(OpenUpdateProject);
        }
        #region Properties
        private List<ModelClient<ProjectModel>> userProjects = new List<ModelClient<ProjectModel>>();
        public List<ModelClient<ProjectModel>> UserProjects
        {
            get { return projectsRequestService.GetAllProjects(token).Select(p => new ModelClient<ProjectModel>(p)).ToList(); }
            set { userProjects = value; RaisePropertyChanged(nameof(userProjects)); }
        }
        public ModelClient<ProjectModel> selectedProject;
        public ModelClient<ProjectModel> SelectedProject
        {
            get { return selectedProject; }
            set { selectedProject = value; RaisePropertyChanged(nameof(SelectedProject)); }
        }
        private List<UserModel> projectUsers;
        public List<UserModel> ProjectUsers
        {
            get => projectUsers;
            set
            {
                projectUsers = value;
                RaisePropertyChanged(nameof(projectUsers));
                if (SelectedProject != null || SelectedProject.Model.AllUsersIds.Count > 0)
                {
                    ProjectUsers = SelectedProject.Model.AllUsersIds.Select(userId => usersRequestService.GetUserById(token, userId)).ToList();
                }
            }

        }

        #endregion
        #region Methods
        private void OpenNewProject()
        {
            commonViewService.ShowMessage(nameof(OpenNewProject));
        }
        private void ShowProjectInfo(object param)
        {
            var selectedProject = param as ModelClient<ProjectModel>;
            SelectedProject = selectedProject;
            commonViewService.ShowMessage(nameof(ShowProjectInfo));
        }
        private void OpenUpdateProject(object param)
        {
            var selectedProject = param as ModelClient<ProjectModel>;
            SelectedProject = selectedProject;
            commonViewService.ShowMessage(nameof(OpenUpdateProject));
        }
        #endregion

    }
}
