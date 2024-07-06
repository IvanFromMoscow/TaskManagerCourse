using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Services
{
    public class DeskService : AbstarctService, ICommonService<DeskModel>
    {
        private readonly ApplicationContext db;
        public DeskService(ApplicationContext applicationContext)
        {
            db = applicationContext;
        }
        public bool Create(DeskModel model)
        {
            return DoAction(() =>
            {
                Desk newDesk = new Desk(model);
                db.Desks.Add(newDesk);
                db.SaveChanges();
            });
        }

        public bool Delete(int id)
        {
            return DoAction(() =>
            {
                Desk deskForDelete = db.Desks.FirstOrDefault(d => d.Id == id);
                db.Desks.Remove(deskForDelete);
                db.SaveChanges();
            });
        }

        public DeskModel Get(int id)
        {
            var desk = db.Desks
                .Include(d => d.Tasks)
                .FirstOrDefault(d => d.Id == id);
            var deskModel = desk?.ToDto();
            if (deskModel != null)
            {
                deskModel.TasksIds = desk?.Tasks.Select(t => t.Id).ToList()!;
            }
            return deskModel!;
        }

        public bool Update(int id, DeskModel model)
        {
            return DoAction(() =>
            {
                Desk desk = db.Desks.FirstOrDefault(d => d.Id == id)!;
                desk.Name = model.Name;
                desk.Description = model.Description;
                desk.Photo = model.Photo;
                desk.AdminId = model.AdminId;
                desk.IsPrivate = model.IsPrivate;
                desk.Columns = JsonConvert.SerializeObject(model.Columns);

                db.Desks.Update(desk);
                db.SaveChanges();
            });
        }
        public IQueryable<CommonModel> GetAll(int userId)
        {
            return db.Desks.Where(d => d.AdminId == userId).Select(d => d.ToDto() as CommonModel);
        }

        public IQueryable<CommonModel> GetProjectDesks(int projectId, int userId)
        {

            return db.Desks.Where(d => 
                d.ProjectId == projectId && 
                (d.IsPrivate == false || d.AdminId == userId))
            .Select(d => d.ToDto() as CommonModel);
        }
       
    }
}
