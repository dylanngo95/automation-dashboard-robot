using AutomationDashboard.Data.Infrastructure;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data.Repositories
{
    public interface IBuildRepository : IRepository<Build> {

        List<Build> GetAllBuild(int subProjectId, int size);

        Build GetLastBuild(int subProjectId);

        List<Build> GetLast7Build(int subProjectId);

    }

    public class BuildRepository : RepositoryBase<Build>, IBuildRepository
    {
        public BuildRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<Build> GetAllBuild(int subProjectId, int size)
        {
            return this.DbContext.Builds.Where(p => p.SubProjectId == subProjectId).Take(size).ToList();
        }

        public List<Build> GetLast7Build(int subProjectId)
        {
            return this.DbContext.Builds.Where(p => p.SubProjectId == subProjectId).OrderByDescending(p => p.FinishDate).Take(7).ToList();
        }

        public Build GetLastBuild(int subProjectId)
        {
            return this.DbContext.Builds.Where(p => p.SubProjectId == subProjectId).OrderByDescending(p => p.FinishDate).FirstOrDefault();
        }
    }
}
