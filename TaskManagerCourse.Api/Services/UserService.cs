using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Services
{
    public class UserService : AbstarctService, ICommonService<UserModel>
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
        public User GetUser(string login)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == login);
            return user;
        }
        public ClaimsIdentity GetIdentity(string username, string password)
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

        public bool Create(UserModel model)
        {
            try
            {
                User newUser = new User(
                    model.FirstName, model.LastName,
                    model.Email,
                    model.Password,
                    model.Status,
                    model.Phone,
                    model.Photo);
                db.Users.Add(newUser);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(int id, UserModel model)
        {

            User userForUpdate = db.Users.FirstOrDefault(x => x.Id == id);
            if (userForUpdate != null)
            {
                return DoAction(() =>
                {
                    userForUpdate.FirstName = model.FirstName;
                    userForUpdate.LastName = model.LastName;
                    userForUpdate.Email = model.Email;
                    userForUpdate.Password = model.Password;
                    userForUpdate.Status = model.Status;
                    userForUpdate.Phone = model.Phone;
                    userForUpdate.Photo = model.Photo;

                    db.Users.Update(userForUpdate);
                    db.SaveChanges();
                });
            }
            return false;
        }

        public bool Delete(int id)
        {
            User userForDelete = db.Users.FirstOrDefault(x => x.Id == id);
            if (userForDelete != null)
            {
                return DoAction(() =>
                {
                    db.Users.Remove(userForDelete);
                    db.SaveChanges();
                });
            }
            return false;
        }
        public bool CreateMultipleUsers(List<UserModel> userModels)
        {
            return DoAction(() =>
            {
                var newUsers = userModels.Select(u => new User(u)).ToList();
                db.Users.AddRange(newUsers);
                db.SaveChangesAsync();
            });
        }

        public UserModel Get(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            return user?.ToDto()!;
        }

        public IEnumerable<UserModel> GetAllByIds(List<int> userIds)
        {
            foreach (var id in userIds)
            {
                var user = db.Users.FirstOrDefault(u => u.Id == id)?.ToDto();
                yield return user;
            }
        }
    }
}
