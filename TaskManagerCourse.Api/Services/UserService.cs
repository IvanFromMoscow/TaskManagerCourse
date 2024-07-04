using System.Security.Claims;
using System.Text;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;

namespace TaskManagerCourse.Api.Services
{
    public class UserService
    {
        private readonly ApplicationContext db;
        public UserService(ApplicationContext applicationContext)
        {
            db = applicationContext;
        }

        public Tuple<string, string> GetUserLoginPassFromBasicAuth(HttpRequest request)
        {
            string userName = string.Empty;
            string userPassword = string.Empty;
            string authHeader = request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith("Basic")) 
            {
                string encodedUserNamePass = authHeader.Replace("Basic", "");
                var encoding = Encoding.GetEncoding("iso-8859-1");

                string[] namePassArray = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(':');
                userName = namePassArray[0];
                userPassword = namePassArray[1];
            }
            return new Tuple<string, string>(userName, userPassword);
        }

        public User GetUser(string login, string password) 
        {
            var user = db.Users.FirstOrDefault(u => u.Email == login && u.Password == password);
            return user;
        }

        public ClaimsIdentity GetIdentity(string username,  string password)
        {
            User currentUser = GetUser(username, password);
            if (currentUser != null) 
            { 
                currentUser.LastLoginDate = DateTime.Now;
                db.Users.Update(currentUser);
                db.SaveChanges();

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, currentUser.Status.ToString()),
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token",
                    ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}
