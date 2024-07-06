using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Common.Models;
using Task = TaskManagerCourse.Api.Models.Task;


namespace TaskManagerCourse.Api.Services
{
    public class TaskService : AbstarctService, ICommonService<TaskModel>
    {

        private readonly ApplicationContext db;
        public TaskService(ApplicationContext applicationContext)
        {
            db = applicationContext;
        }
        public bool Create(TaskModel model)
        {
            return DoAction(() =>
            {
                Task newTask = new Task(model);
                db.Tasks.Add(newTask);
                db.SaveChanges();
            });
        }

        public bool Delete(int id)
        {
            return DoAction(() =>
            {
                Task taskForDelete = db.Tasks.FirstOrDefault(t => t.Id == id);
                if (taskForDelete != null)
                {
                    db.Tasks.Remove(taskForDelete);
                    db.SaveChanges();
                }
            });
        }

        public TaskModel Get(int id)
        {
            Task task = db.Tasks.FirstOrDefault(t => t.Id == id)!;
            return task?.ToDto()!;
        }

        public bool Update(int id, TaskModel model)
        {
            return DoAction(() =>
            {
                Task task = db.Tasks.FirstOrDefault(t => t.Id == id)!;
                
                task.Name = model.Name;
                task.Description = model.Description;
                task.Photo = model.Photo;
                task.StartDate = model.CreationDate;
                task.EndDate = model.EndDate;
                task.File = model.File;
                task.DeskId = model.DeskId;
                task.Column = model.Column;
                task.CreatorId = model.CreatorId;
                task.ExecutorId = model.ExecutorId;

                db.Tasks.Update(task);
                db.SaveChanges();
            });
        }

        public IQueryable<CommonModel> GetAll(int deskId)
        {
            return db.Tasks.Where(t => t.DeskId == deskId).Select(t => t.ToShortDto());
        }

        public IQueryable<CommonModel> GetTaskForUser(int userId)
        {
            var tasks = db.Tasks.Where(t => t.CreatorId == userId || t.ExecutorId == userId)
                .Select(t => t.ToDto() as CommonModel);
            return tasks;
        }

    }
}
