using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerCourse.Common.Models
{
    public class ProjectModel : CommonModel
    {
        public int? AdminId {  get; set; }
        public ProjectStatus Status { get; set; }
        public List<int> AllUsersIds { get; set; } = new();
        public List<int> AllDesksIds { get; set; } = new();
        public ProjectModel(string name, string description,
                   ProjectStatus status = ProjectStatus.InProgress,
                   byte[]? photo = null)
        {
            Name = name;
            Description = description;
            CreationDate = DateTime.Now;
            Photo = photo;
            Status = status;
        }
        public ProjectModel() { }
    }
}
