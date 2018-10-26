using AutomationDashboard.Data.Infrastructure;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data.Repositories
{
    public interface IProjectRepository : IRepository<Project> {

        List<Project> GetProjectActives();
    }

    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<Project> GetProjectActives()
        {
            return this.DbContext.Projects.Where(p => p.Activate).ToList();
        }
    }
}
