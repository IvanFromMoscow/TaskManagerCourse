using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        public DelegateCommand<object> LoginFromCacheCommand { get; private set; }
        #endregion
        public LoginViewModel()
        {
            usersRequestService = new UsersRequestService();
            CurrentUserCache = GetUserCache();
            GetUserFromDBCommand = new DelegateCommand<object>(GetUserFromDB);
            LoginFromCacheCommand = new DelegateCommand<object>(LoginFromCache);
        }

        #region Properties
        private string cachePath = Path.GetTempPath() + "usertaskmanagercourse.txt";
        private Window currentWindow;

        public UserCache currentUserCache;
        public UserCache CurrentUserCache
        {
            get { return currentUserCache; }
            set { currentUserCache = value; RaisePropertyChanged(nameof(CurrentUserCache)); }
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
        #endregion

        #region Methods
        private void GetUserFromDB(object parameter)
        {
            var passBox = parameter as PasswordBox;
            currentWindow = Window.GetWindow(passBox);
            bool isNewUser = false;

            if (UserLogin != CurrentUserCache?.Login ||
                UserPassword != CurrentUserCache?.Password)
            {
                isNewUser = true;
            }


            UserPassword = passBox.Password;
            AuthToken = usersRequestService.GetToken(UserLogin, UserPassword);
            if (AuthToken == null)
            {
                return;
            }
            CurrentUser = usersRequestService.GetCurrentUser(AuthToken);
            if (CurrentUser != null)
            {
                if (isNewUser)
                {
                    var saveUserCache = MessageBox.Show("Хотите сохранить логин и пароль?", "Сохранение данных", MessageBoxButton.YesNo);
                    if (saveUserCache == MessageBoxResult.Yes)
                    {
                        UserCache newUserCache = new UserCache()
                        {
                            Login = UserLogin,
                            Password = UserPassword,
                        };
                        CreateUserCache(newUserCache);
                    }
                }
                OpenMainWindow();
            }
        }
        private void CreateUserCache(UserCache userCache)
        {
            string jsonUserCache = JsonConvert.SerializeObject(userCache);
            using (StreamWriter sw = new StreamWriter(cachePath, false, Encoding.Default))
            {
                sw.Write(jsonUserCache);
                MessageBox.Show("Success!");
            }
        }
        private UserCache GetUserCache()
        {
            bool isCacheExist = File.Exists(cachePath);
            if (isCacheExist && File.ReadAllText(cachePath).Length > 0)
            {
                return JsonConvert.DeserializeObject<UserCache>(File.ReadAllText(cachePath));
            }
            return null;
        }

        private void LoginFromCache(object wnd)
        {
            currentWindow = wnd as Window;
            UserLogin = CurrentUserCache.Login;
            UserPassword = CurrentUserCache.Password;
            AuthToken = usersRequestService.GetToken(UserLogin, UserPassword);

            CurrentUser = usersRequestService.GetCurrentUser(AuthToken);
            if (CurrentUser != null)
            {
                OpenMainWindow();
            }
        }

        private void OpenMainWindow()
        {
            MainWindow window = new MainWindow();
            window.DataContext = new MainWindowViewModel(AuthToken, CurrentUser, window);

            window.Show();
            currentWindow.Close();
        }
        #endregion
    }
}
