using Microsoft.EntityFrameworkCore;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Services
{
    public class ProjectService : AbstarctService, ICommonService<ProjectModel>
    {
        private readonly ApplicationContext db;
        public ProjectService(ApplicationContext applicationContext)
        {
            db = applicationContext;
        }
        public bool Create(ProjectModel model)
        {
            bool result = DoAction(() =>
            {
                Project project = new Project(model);
                db.Projects.Add(project);
                db.SaveChanges();
            });
            return result;
        }

        public bool Delete(int id)
        {
            bool result = DoAction(() =>
            {
                Project project = db.Projects.FirstOrDefault(p => p.Id == id);
                db.Projects.Remove(project);
                db.SaveChanges();
            });
            return result;
        }

        public ProjectModel Get(int id)
        {
            Project project = db.Projects.Include(p => p.AllUsers).FirstOrDefault(p => p.Id == id);
            var projectModel = project?.ToDto();
            if (projectModel != null)
            {
                projectModel.AllUsersIds = project.AllUsers.Select(u => u.Id).ToList();
            }
            return projectModel;
        }

        public async Task<IEnumerable<ProjectModel>> GetByUserId(int userId)
        {
            List<ProjectModel> result = new List<ProjectModel>();
            var admin = db.ProjectAdmins.FirstOrDefault(a => a.UserId == userId);
            if (admin != null) 
            {
                var projectsForAdmin = await db.Projects.Where(p => p.AdminId == admin.Id)
                    .Select(p => p.ToDto()).ToListAsync();
                result.AddRange(projectsForAdmin);
            }
            var projectsForUser = await db.Projects
                .Include(p => p.AllUsers)
                . Where(p => p.AllUsers.Any(u => u.Id == userId))
                .Select(p => p.ToDto()).ToListAsync();
            result.AddRange(projectsForUser);
            return result;
        }

        public bool Update(int id, ProjectModel model)
        {
            bool result = DoAction(() =>
            {
                Project project = db.Projects.FirstOrDefault(p => p.Id == id);
                project.Name = model.Name;
                project.Description = model.Description;
                project.Photo = model.Photo;
                project.Status = model.Status;
                project.AdminId = model.AdminId;
                db.Projects.Update(project);
                db.SaveChanges();
            });
            return result;
        }
        public IQueryable<CommonModel> GetAll()
        {
            return db.Projects.Select(p => p.ToDto() as CommonModel);
        }
        public void AddUsersToProject(int id, IEnumerable<int> userIds)
        {
            Project project = db.Projects.FirstOrDefault(p => p.Id == id);
            foreach (int userId in userIds)
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                project.AllUsers.Add(user);
                
            }
            db.SaveChanges();
        }
        public bool RemoveUsersFromProject(int id, IEnumerable<int> userIds)
        {
            Project project = db.Projects.Include(p => p.AllUsers).FirstOrDefault(p => p.Id == id);
            bool result = false;
            foreach (int userId in userIds)
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (project.AllUsers.Contains(user))
                {
                    project.AllUsers.Remove(user);
                    result = true;
                }
            }
            if (result)
            {
                db.SaveChanges();
            }
            return result;
        }
    }
}
