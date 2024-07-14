using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Services;
using TaskManagerCourse.Client.Views;
using TaskManagerCourse.Client.Views.Pages;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private CommonViewService viewService;
        #region Commands
        public DelegateCommand OpenProjectsPageCommand { get; private set; }
        public DelegateCommand OpenTasksPageCommand { get; private set; }
        public DelegateCommand OpenDesksPageCommand { get; private set; }
        public DelegateCommand OpenMyInfoPageCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand OpenUsersManagerPageCommand { get; private set; }

        #endregion
        public MainWindowViewModel(AuthToken token, UserModel currentUser, Window currentWindow = null)
        {
            viewService = new CommonViewService();
            
            Token = token;
            CurrentUser = currentUser;
            this.currentWindow = currentWindow;

            OpenProjectsPageCommand = new DelegateCommand(OpenProjectsPage);
            navButtons.Add(userProjectsBtnName, OpenProjectsPageCommand);
            
            OpenTasksPageCommand = new DelegateCommand(OpenTasksPage);
            navButtons.Add(userTaskBtnName, OpenTasksPageCommand);

            OpenDesksPageCommand = new DelegateCommand(OpenDesksPage);
            navButtons.Add(userDesksBtnName, OpenDesksPageCommand);

            OpenMyInfoPageCommand = new DelegateCommand(OpenMyInfoPage);
            navButtons.Add(userInfoBtnName, OpenMyInfoPageCommand);

            if(currentUser != null && currentUser.Status == UserStatus.Admin)
            {
                OpenUsersManagerPageCommand = new DelegateCommand(OpenUsersManagerPage);
                navButtons.Add(usersManagerBtnName, OpenUsersManagerPageCommand);
            }

            LogoutCommand = new DelegateCommand(Logout);
            navButtons.Add(userLogoutBtnName, LogoutCommand);

            OpenMyInfoPage();
        }
        #region Properties
        private readonly string userProjectsBtnName = "My projects";
        private readonly string userDesksBtnName = "My desks";
        private readonly string userTaskBtnName = "My tasks";
        private readonly string userInfoBtnName = "My Info";
        private readonly string userLogoutBtnName = "Logout";
        private readonly string usersManagerBtnName = "Users";

        private Dictionary<string, DelegateCommand> navButtons = new Dictionary<string, DelegateCommand>();
        public Dictionary<string, DelegateCommand> NavButtons
        {
            get { return navButtons; }
            set { navButtons = value; RaisePropertyChanged(nameof(navButtons)); }
        }
        private AuthToken token;
        public AuthToken Token
        {
            get { return token; }
            set { token = value; RaisePropertyChanged(nameof(Token)); }
        }
        private UserModel currentUser;
        public UserModel CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; RaisePropertyChanged(nameof(CurrentUser)); }
        }
        private string selectedPageName;
        public string SelectedPageName
        {
            get { return selectedPageName; }
            set { selectedPageName = value; RaisePropertyChanged(nameof(SelectedPageName)); }
        }

        public Page selectedPage;
        public Page SelectedPage
        {
            get { return selectedPage; }
            set { selectedPage = value; RaisePropertyChanged(nameof(SelectedPage)); }
        }
        private Window currentWindow;
        #endregion

        #region Methods
        private void OpenProjectsPage()
        {
            var page = new ProjectsPage();
            OpenPage(page, userProjectsBtnName, new ProjectsPageViewModel(Token));
            SelectedPageName = userProjectsBtnName;
        }
        private void OpenTasksPage()
        {
            var page = new UsersTasksPage();
            OpenPage(page, userTaskBtnName, new UsersTasksPageViewModel(Token));
        }
        private void OpenDesksPage()
        {
            SelectedPageName = userDesksBtnName;
            viewService.ShowMessage(userDesksBtnName);
        }
        private void OpenMyInfoPage()
        {
            var page = new UserInfoPage();
            OpenPage(page, userProjectsBtnName, this);
        }
        private void OpenUsersManagerPage()
        {
            SelectedPageName = usersManagerBtnName;
            viewService.ShowMessage(usersManagerBtnName);
        }
        private void Logout()
        {
            var question = MessageBox.Show("Are you sure?", "Logout", MessageBoxButton.YesNo);
            if(question == MessageBoxResult.Yes && currentWindow != null)
            {
                Login login = new Login();
                login.Show();
                currentWindow.Close();
            }
        }
        #endregion
        private void OpenPage(Page page, string pageName = null, BindableBase viewModel = null)
        {
            SelectedPage = page;
            SelectedPageName = pageName;
            SelectedPage.DataContext = viewModel;
        }
    }
}
