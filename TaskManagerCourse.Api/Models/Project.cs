namespace TaskManagerCourse.Api.Models
{
    public class Project : CommonObject
    {
        public int Id { get; set; }
        public int? AdminId { get; set; }
        public ProjectAdmin Admin { get; set; }
        public ProjectStatus Status { get; set; }
        public List<User> AllUsers { get; set; } = new();
        public List<Desk> AllDesks { get; set; } = new();
    }
}
