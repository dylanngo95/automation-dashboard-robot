using AutomationDashboard.Data.Infrastructure;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data.Repositories
{
    public interface IRequestBuildDetailLogRepository : IRepository<RequestBuildDetailLog> {

    }

    public class RequestBuildDetailLogRepository : RepositoryBase<RequestBuildDetailLog>, IRequestBuildDetailLogRepository
    {
        public RequestBuildDetailLogRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
