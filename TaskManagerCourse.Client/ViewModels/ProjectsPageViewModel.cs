using DryIoc.ImTools;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Services;
using TaskManagerCourse.Client.Views.AddWindows;
using TaskManagerCourse.ClientTests.Services;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.ViewModels
{
    public class ProjectsPageViewModel : BindableBase
    {
        #region Commands
        public DelegateCommand OpenNewProjectCommand { get; private set; }
        public DelegateCommand<object> ShowProjectInfoCommand { get; private set; }
        public DelegateCommand<object> OpenUpdateProjectCommand { get; private set; }
        public DelegateCommand CreateOrUpdateProjectCommand { get; private set; }
        public DelegateCommand DeleteProjectCommand { get; private set; }
        public DelegateCommand SelectPhotoForProjectCommand { get; private set; }
        public DelegateCommand AddUsersToProjectCommand { get; private set; }
        public DelegateCommand OpenNewUsersToProjectCommand { get; private set; }

        #endregion
        private AuthToken token;
        private UsersRequestService usersRequestService;
        private ProjectsRequestService projectsRequestService;
        private CommonViewService commonViewService;
        public ProjectsPageViewModel(AuthToken token = null)
        {

            usersRequestService = new UsersRequestService();
            commonViewService = new CommonViewService();
            projectsRequestService = new ProjectsRequestService();
            this.token = token;
            UpdatePage();
            OpenNewProjectCommand = new DelegateCommand(OpenNewProject);
            ShowProjectInfoCommand = new DelegateCommand<object>(ShowProjectInfo);
            OpenUpdateProjectCommand = new DelegateCommand<object>(OpenUpdateProject);
            CreateOrUpdateProjectCommand = new DelegateCommand(CreateOrUpdateProject);
            DeleteProjectCommand = new DelegateCommand(DeleteProject);
            SelectPhotoForProjectCommand = new DelegateCommand(SelectPhotoForProject);
            AddUsersToProjectCommand = new DelegateCommand(AddUsersToProject);
            OpenNewUsersToProjectCommand = new DelegateCommand(OpenNewUsersToProject);
        }




        #region Properties
        public UserModel CurrentUser
        {
            get => usersRequestService.GetCurrentUser(token);
        }
        private List<ModelClient<ProjectModel>> userProjects = new List<ModelClient<ProjectModel>>();
        public List<ModelClient<ProjectModel>> UserProjects
        {
            get { return userProjects; }
            set { userProjects = value; RaisePropertyChanged(nameof(userProjects)); }
        }
        public ModelClient<ProjectModel> selectedProject;
        public ModelClient<ProjectModel> SelectedProject
        {
            get { return selectedProject; }
            set
            {
                selectedProject = value;
                RaisePropertyChanged(nameof(SelectedProject));
                if (SelectedProject != null && SelectedProject.Model.AllUsersIds.Count > 0)
                {
                    ProjectUsers = SelectedProject.Model.AllUsersIds.Select(userId => usersRequestService.GetUserById(token, userId)).ToList();
                }
                else
                {
                    ProjectUsers = new List<UserModel>();
                }
            }
        }
        private List<UserModel> projectUsers;
        public List<UserModel> ProjectUsers
        {
            get => projectUsers;
            set { projectUsers = value; RaisePropertyChanged(nameof(ProjectUsers)); }
        }
        private ClientAction typeActionWithProject;
        public ClientAction TypeActionWithProject
        {
            get
            {
                return typeActionWithProject;
            }
            set { typeActionWithProject = value; RaisePropertyChanged(nameof(TypeActionWithProject)); }
        }
        public List<UserModel> NewUsersForSelectedProject
        {
            get => usersRequestService.GetAllUsers(token).Where(user => projectUsers.Any(u => u.Id == user.Id) == false).ToList();
        }
        public List<UserModel> selectedUsersForProject = new();
        public List<UserModel> SelectedUsersForProject
        {
            get { return selectedUsersForProject; }
            set { selectedUsersForProject = value; RaisePropertyChanged(nameof(SelectedUsersForProject)); }
        }

        #endregion
        #region Methods
        private void OpenNewProject()
        {
            TypeActionWithProject = ClientAction.Create;
            commonViewService.OpenWindow(new CreateOrUpdateProjectWindow(), this);
            commonViewService.ShowMessage(nameof(OpenNewProject));
        }

        private void ShowProjectInfo(object param)
        {
            SelectedProject = GetModelClientById(param);
        }

        private ModelClient<ProjectModel> GetModelClientById(object param)
        {
            var selectedProject = param as ModelClient<ProjectModel>;
            var project = projectsRequestService.GetProjectById(token, selectedProject?.Model?.Id ?? 0);
            return new ModelClient<ProjectModel>(project);
        }

        private void OpenUpdateProject(object param)
        {
            TypeActionWithProject = ClientAction.Update;
            SelectedProject = GetModelClientById(param);

            var wnd = new CreateOrUpdateProjectWindow();
            commonViewService.OpenWindow(new CreateOrUpdateProjectWindow(), this);


        }

        private void CreateProject()
        {
            var resultAction = projectsRequestService.CreateProject(token, SelectedProject.Model);
            commonViewService.ShowActionResult(resultAction, "New project is created!");
        }
        private void UpdateProject()
        {
            var resultAction = projectsRequestService.UpdateProject(token, SelectedProject.Model);
            commonViewService.ShowActionResult(resultAction, "New project is updated!");

        }
        private void DeleteProject()
        {
            var resultAction = projectsRequestService.DeleteProject(token, SelectedProject.Model.Id);
            commonViewService.ShowActionResult(resultAction, "New project is deleted!");
            UpdatePage();
        }
        private void CreateOrUpdateProject()
        {
            if (TypeActionWithProject == ClientAction.Create)
            {
                CreateProject();
            }
            if (TypeActionWithProject == ClientAction.Update)
            {
                UpdateProject();
            }
            UpdatePage();
        }
        private List<ModelClient<ProjectModel>> GetUserProjectClient()
        {
            commonViewService.CurrentOpenWindow?.Close();
            return projectsRequestService.GetAllProjects(token).Select(p => new ModelClient<ProjectModel>(p)).ToList(); ;
        }
        private void SelectPhotoForProject()
        {
            commonViewService.SetPhotoForObject(SelectedProject.Model);
            SelectedProject = new ModelClient<ProjectModel>(SelectedProject.Model);
        }
        private void AddUsersToProject()
        {
            if (SelectedUsersForProject == null || SelectedUsersForProject.Count == 0)
            {
                commonViewService.ShowMessage("Select no users");
                return;
            }
            var resultAction = projectsRequestService.AddUsersToProject(token, SelectedProject.Model.Id, SelectedUsersForProject.Select(u => u.Id).ToList());
            commonViewService.ShowActionResult(resultAction, "New users are added to project");

            UpdatePage();
        }
        private void OpenNewUsersToProject()
        {
            var wnd = new AddUsersToProjectWindow();
            commonViewService.OpenWindow(wnd, this);
        }
        private void UpdatePage()
        {
            UserProjects = GetUserProjectClient();
            SelectedProject = null;
            SelectedUsersForProject = new();
        }

        #endregion

    }
}
