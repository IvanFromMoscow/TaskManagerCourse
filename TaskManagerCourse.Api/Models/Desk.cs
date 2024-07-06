using Newtonsoft.Json;
using System.Net.NetworkInformation;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Models
{
    public class Desk : CommonObject
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public string Columns{ get; set; }
        public int AdminId { get; set; }
        public User Admin { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public List<TaskModel> Tasks { get; set; } = new();

        public Desk(DeskModel deskModel) : base(deskModel)
        {
            Id = deskModel.Id;
            AdminId = deskModel.AdminId;
            IsPrivate = deskModel.IsPrivate;
            ProjectId = deskModel.ProjectId;
            if (deskModel.Columns.Any())
            {
                Columns = JsonConvert.SerializeObject(deskModel.Columns);
            }
        }
        public DeskModel ToDto()
        {
            return new DeskModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreationDate = this.CreationDate,
                Photo = this.Photo,
                AdminId = this.AdminId,
                IsPrivate = this.IsPrivate,
                ProjectId = this.ProjectId,
                Columns = JsonConvert.DeserializeObject<string[]>(this.Columns)
            };
        }
        public Desk()
        {
            
        }
    }
}
