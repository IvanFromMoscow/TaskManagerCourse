using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Views;
using TaskManagerCourse.Client.Views.Pages;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        #region Commands
        public DelegateCommand OpenProjectsPageCommand;
        public DelegateCommand OpenTasksPageCommand;
        public DelegateCommand OpenDesksPageCommand;
        public DelegateCommand OpenMyInfoPageCommand;
        public DelegateCommand LogoutCommand;
        public DelegateCommand OpenUsersManagerPageCommand;

        #endregion
        public MainWindowViewModel(AuthToken token, UserModel currentUser, Window currentWindow = null)
        {
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
            SelectedPageName = userProjectsBtnName;
            ShowMessage(userProjectsBtnName);
        }
        private void OpenTasksPage()
        {
            var page = new UsersTasksPage();
            OpenPage(page, userTaskBtnName, new UsersTasksPageViewModel(Token));
        }
        private void OpenDesksPage()
        {
            SelectedPageName = userDesksBtnName;
            ShowMessage(userDesksBtnName);
        }
        private void OpenMyInfoPage()
        {
            var page = new UserInfoPage();
            OpenPage(page, userProjectsBtnName, this);
        }
        private void OpenUsersManagerPage()
        {
            SelectedPageName = usersManagerBtnName;
            ShowMessage(usersManagerBtnName);
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
        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        private void OpenPage(Page page, string pageName, BindableBase viewModel)
        {
            SelectedPage = page;
            SelectedPageName = pageName;
            SelectedPage.DataContext = viewModel;
        }
    }
}
