using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerCourse.Client.Models;
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
        public MainWindowViewModel(AuthToken token, UserModel currentUser)
        {
            Token = token;
            CurrentUser = currentUser;

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
                
        }
        #region Properties
        private readonly string userProjectsBtnName = "My projects";
        private readonly string userDesksBtnName = "My desks";
        private readonly string userTaskBtnName = "My tasks";
        private readonly string userInfoBtnName = "My Info";
        private readonly string userLogoutBtnName = "Logout";
        private readonly string usersManagerBtnName = "Users";

        public Dictionary<string, DelegateCommand> navButtons = new Dictionary<string, DelegateCommand>();
        public Dictionary<string, DelegateCommand> NavButtons
        {
            get { return navButtons; }
            set { navButtons = value; RaisePropertyChanged(nameof(navButtons)); }
        }
        public AuthToken token;
        public AuthToken Token
        {
            get { return token; }
            set { token = value; RaisePropertyChanged(nameof(Token)); }
        }
        public UserModel currentUser;
        public UserModel CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; RaisePropertyChanged(nameof(CurrentUser)); }
        }


        #endregion

        #region Methods
        private void OpenProjectsPage()
        {
            ShowMessage(userProjectsBtnName);
        }
        private void OpenTasksPage()
        {
            ShowMessage(userTaskBtnName);
        }
        private void OpenDesksPage()
        {
            ShowMessage(userDesksBtnName);
        }
        private void OpenMyInfoPage()
        {
            ShowMessage(userInfoBtnName);
        }
        private void OpenUsersManagerPage()
        {
            ShowMessage(usersManagerBtnName);
        }
        private void Logout()
        {
            ShowMessage(userLogoutBtnName);
        }
        #endregion
        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
