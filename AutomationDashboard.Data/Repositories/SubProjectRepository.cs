using AutomationDashboard.Data.Infrastructure;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data.Repositories
{
    public interface ISubProjectRepository : IRepository<SubProject> {

        List<SubProject> GetSubProjectActiveByProjectId(int projectId);

        SubProject GetSubProjectById(int projectId);

        List<SubProject> GetSubProjectAndCheckActive();
    }

    public class SubProjectRepository : RepositoryBase<SubProject>, ISubProjectRepository
    {
        public SubProjectRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<SubProject> GetSubProjectActiveByProjectId(int projectId)
        {
            return this.DbContext.SubProjects.Where(p => p.ProjectId == projectId).Where(p => p.Activate).ToList();
        }

        public List<SubProject> GetSubProjectAndCheckActive()
        {
            var subProjects = from p in this.DbContext.Projects
                              join s in this.DbContext.SubProjects on p.Id equals s.ProjectId
                              where p.Activate && s.Activate
                              select s;
            return subProjects.ToList();
        }

        public SubProject GetSubProjectById(int projectId)
        {
            return this.DbContext.SubProjects.SingleOrDefault(p => p.Id == projectId);
        }
    }
}
