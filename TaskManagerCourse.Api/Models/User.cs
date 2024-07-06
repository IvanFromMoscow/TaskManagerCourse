using System.Net.NetworkInformation;
using System.Numerics;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public UserStatus Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public byte[]? Photo { get; set; }
        public List<Project> Projects { get; set; } = new();
        public List<Desk> Desks { get; set; } = new();
        public List<Task> Tasks { get; set; } = new();

        public User() { }

        public User(string fname, string lname, string email, string password,
                    UserStatus status = UserStatus.User,
                    string? phone = null,
                    byte[]? photo = null)
        {
            FirstName = fname;
            LastName = lname;
            Email = email;
            Password = password;
            Phone = phone;
            Status = status;
            Photo = photo;
            RegistrationDate = DateTime.Now;

        }
        public User(UserModel userModel)
        {
            FirstName = userModel.FirstName;
            LastName = userModel.LastName;
            Email = userModel.Email;
            Password = userModel.Password;
            Phone = userModel.Phone;
            Status = userModel.Status;
            Photo = userModel.Photo;
            RegistrationDate = userModel.RegistrationDate;

        }
        public UserModel ToDto()
        {
            return new UserModel()
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                Password = this.Password,
                Phone = this.Phone,
                Photo = this.Photo,
                Status = this.Status,
                RegistrationDate = this.RegistrationDate,
        };
    }
}
}
