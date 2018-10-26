using AutomationDashboard.Data.Infrastructure;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data.Repositories
{
    public interface ICronJobSettingRepository : IRepository<CronJobSetting> {

        CronJobSetting GetCronJobSettingFirst();

    }

    public class CronJobSettingRepository : RepositoryBase<CronJobSetting>, ICronJobSettingRepository
    {
        public CronJobSettingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public CronJobSetting GetCronJobSettingFirst()
        {
            return this.DbContext.CronJobSettings.FirstOrDefault();
        }
    }
}
