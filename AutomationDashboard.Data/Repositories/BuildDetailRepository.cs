using AutomationDashboard.Data.Infrastructure;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data.Repositories
{
    public interface IBuildDetailRepository : IRepository<BuildDetail> {


    }

    public class BuildDetailRepository : RepositoryBase<BuildDetail>, IBuildDetailRepository
    {
        public BuildDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

    }
}
