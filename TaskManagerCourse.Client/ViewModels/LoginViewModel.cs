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
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        UsersRequestService usersRequestService;

        #region Commands
        public DelegateCommand<object> GetUserFromDBCommand { get; private set; }
        #endregion
        public LoginViewModel()
        {
            usersRequestService = new UsersRequestService();
            GetUserFromDBCommand = new DelegateCommand<object>(GetUserFromDB);
        }

        public string UserLogin { get; set; }
        public string UserPassword { get; private set; }
        private UserModel currentUser;
        public UserModel CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; RaisePropertyChanged(nameof(CurrentUser)); }
        }
        private AuthToken authToken;
        public AuthToken AuthToken
        {
            get { return authToken; }
            set { authToken = value; RaisePropertyChanged(nameof(AuthToken)); }
        }

        #region Methods
        private void GetUserFromDB(object parameter)
        {
            var passBox = parameter as PasswordBox;
            UserPassword = passBox.Password;
            AuthToken = usersRequestService.GetToken(UserLogin, UserPassword);
            if (AuthToken != null) 
            {
                CurrentUser = usersRequestService.GetCurrentUser(AuthToken);
                if (CurrentUser != null)
                {
                    MessageBox.Show(CurrentUser.FirstName);
                }
            }
        }
        #endregion
    }
}
