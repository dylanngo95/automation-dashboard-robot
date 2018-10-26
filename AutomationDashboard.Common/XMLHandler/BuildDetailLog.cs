using AutomationDashboard.Data;
using AutomationDashboard.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Common.XMLHandler
{
    public abstract class BuildDetailLog
    {
        public void AddRequestBuildDetail(int buildId, string note, AutomationDashboardDbContext db)
        {
            db.RequestBuildDetailLogs.Add(new RequestBuildDetailLog()
            {
                BuidId = buildId,
                StartDate = DateTime.Now,
                Note = note
            });
            db.SaveChanges();
        }

        public void RemoveRequestBuildDetail(int buildId, AutomationDashboardDbContext db)
        {
            var buildDetail = db.RequestBuildDetailLogs.SingleOrDefault(m => m.BuidId == buildId);
            if (buildDetail != null)
            {
                db.RequestBuildDetailLogs.Remove(buildDetail);
                db.SaveChanges();
            }
        }
    }
}
