namespace TaskManagerCourse.Api.Models
{
    public class TaskModel : CommonObject
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[] File { get; set; }
        public User Admin { get; set; }
        public User Interpritator { get; set; }
    }
}
