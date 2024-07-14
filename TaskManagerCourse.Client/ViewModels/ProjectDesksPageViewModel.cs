using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ProjectDesksPageViewModel : BindableBase
    {
        private readonly CommonViewService commonViewService;
        private readonly DesksRequestService desksRequestService;
        private readonly TasksRequestService tasksRequestService;
        private readonly UsersRequestService usersRequestService;


        #region Commands
        public DelegateCommand OpenNewDeskCommand { get; private set; }
        public DelegateCommand<object> OpenUpdateDeskCommand { get; private set; }
        public DelegateCommand CreateOrUpdateDeskCommand { get; private set; }
        public DelegateCommand DeleteDeskCommand { get; private set; }
        public DelegateCommand SelectPhotoForDeskCommand { get; private set; }

        public DelegateCommand<object> RemoveColumnItemCommand { get; private set; }
        public DelegateCommand AddNewColumnItemCommand { get; private set; }
        #endregion
        public ProjectDesksPageViewModel(AuthToken token, ProjectModel project)
        {
            this.token = token;
            this.project = project;
            
            desksRequestService = new DesksRequestService();
            tasksRequestService = new TasksRequestService();
            commonViewService = new CommonViewService();
            usersRequestService = new UsersRequestService();

            UpdatePage();

            OpenNewDeskCommand = new DelegateCommand(OpenNewDesk);
            OpenUpdateDeskCommand = new DelegateCommand<object>(OpenUpdateDesk);
            CreateOrUpdateDeskCommand = new DelegateCommand(CreateOrUpdateDesk);
            DeleteDeskCommand = new DelegateCommand(DeleteDesk);
            RemoveColumnItemCommand = new DelegateCommand<object>(RemoveColumnItem);
            AddNewColumnItemCommand = new DelegateCommand(AddNewColumnItem);
            SelectPhotoForDeskCommand = new DelegateCommand(SelectPhotoForDesk);
        }

        private readonly AuthToken token;
        private readonly ProjectModel project;


        #region Properties
        public List<ModelClient<DeskModel>> projectDesks;
        public List<ModelClient<DeskModel>> ProjectDesks
        {
            get { return projectDesks; }
            set { projectDesks = value; RaisePropertyChanged(nameof(ProjectDesks)); }
        }
        private ClientAction typeActionWithDesk;
        public ClientAction TypeActionWithDesk
        {
            get
            {
                return typeActionWithDesk;
            }
            set { typeActionWithDesk = value; RaisePropertyChanged(nameof(TypeActionWithDesk)); }
        }
        public ModelClient<DeskModel> selectedDesk;
        public ModelClient<DeskModel> SelectedDesk
        {
            get { return selectedDesk; }
            set
            {
                selectedDesk = value;
                RaisePropertyChanged(nameof(SelectedDesk));
                if (SelectedDesk != null && SelectedDesk.Model.TasksIds.Count > 0)
                {
                    DeskTasks = SelectedDesk.Model.TasksIds.Select(taskId => tasksRequestService.GetTaskById(token, taskId)).ToList();
                }
                else
                {
                    DeskTasks = new List<TaskModel>();
                }
            }
        }
        private List<TaskModel> deskTasks;
        public List<TaskModel> DeskTasks
        {
            get => deskTasks;
            set { deskTasks = value; RaisePropertyChanged(nameof(DeskTasks)); }
        }
        private ObservableCollection<ColumnBindingHelp> columnsForNewDesk = new ObservableCollection<ColumnBindingHelp>()
        {
            new ColumnBindingHelp("New"),
            new ColumnBindingHelp("In progres"),
            new ColumnBindingHelp("In review"),
            new ColumnBindingHelp("Completed"),

        };
        public ObservableCollection<ColumnBindingHelp> ColumnsForNewDesk
        {
            get => columnsForNewDesk;
            set 
            {
                columnsForNewDesk = value;
                RaisePropertyChanged(nameof(ColumnsForNewDesk));
            }
        }
        public UserModel CurrentUser
        {
            get => usersRequestService.GetCurrentUser(token);
        }
        #endregion
        #region Methods
        private List<ModelClient<DeskModel>> GetDesks(int projectId)
        {
            var result = new List<ModelClient<DeskModel>>();
            var desks = desksRequestService.GetDesksByProject(token, projectId);
            if (desks != null)
            {
                result = desks?.Select(d => new ModelClient<DeskModel>(d)).ToList();
            }
            return result;
        }
        private void OpenNewDesk()
        {
            throw new NotImplementedException();
        }
        private void CreateDesk()
        {
            SelectedDesk.Model.Columns = ColumnsForNewDesk.Select(c => c.Value).ToArray();
            SelectedDesk.Model.ProjectId = project.Id;

            var resultAction = desksRequestService.CreateDesk(token, SelectedDesk.Model);
            commonViewService.ShowActionResult(resultAction, "New project is created!");
        }
        private void UpdateDesk()
        {
            var resultAction = desksRequestService.UpdateDesk(token, SelectedDesk.Model);
            commonViewService.ShowActionResult(resultAction, "New project is updated!");

        }
        private void DeleteDesk()
        {
            var resultAction = desksRequestService.DeleteDesk(token, SelectedDesk.Model.Id);
            commonViewService.ShowActionResult(resultAction, "New project is deleted!");
            UpdatePage();
        }
        private void CreateOrUpdateDesk()
        {
            if (TypeActionWithDesk == ClientAction.Create)
            {
                CreateDesk();
            }
            if (TypeActionWithDesk == ClientAction.Update)
            {
                UpdateDesk();
            }
            UpdatePage();
        }
        private void UpdatePage()
        {
            SelectedDesk = null;
            ProjectDesks = GetDesks(project.Id);
            commonViewService.CurrentOpenWindow?.Close();
        }
        private void OpenUpdateDesk(object param)
        {
            TypeActionWithDesk = ClientAction.Update;
            SelectedDesk = GetModelClientById(param);

            var wnd = new CreateOrUpdateProjectWindow();
            commonViewService.OpenWindow(new CreateOrUpdateProjectWindow(), this);


        }

        private void RemoveColumnItem(object item)
        {
            ColumnBindingHelp itemToRemove = item as ColumnBindingHelp;
            ColumnsForNewDesk.Remove(itemToRemove);
        }

        private void AddNewColumnItem()
        {
            ColumnsForNewDesk.Add(new ColumnBindingHelp("Column"));
        }
        private ModelClient<DeskModel> GetModelClientById(object param)
        {
            var selectedDesk = param as ModelClient<DeskModel>;
            var project = desksRequestService.GetDeskById(token, selectedDesk?.Model?.Id ?? 0);
            return new ModelClient<DeskModel>(project);
        }
        private void SelectPhotoForDesk()
        {
            commonViewService.SetPhotoForObject(SelectedDesk.Model);
            SelectedDesk = new ModelClient<DeskModel>(SelectedDesk.Model);
        }
        #endregion

    }
}
