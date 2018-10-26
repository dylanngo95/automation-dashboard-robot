using AutomationDashboard.Data.Infrastructure;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data.Repositories
{
    public interface ISettingRepository : IRepository<Setting> {

    }

    public class SettingRepository : RepositoryBase<Setting>, ISettingRepository
    {
        public SettingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
