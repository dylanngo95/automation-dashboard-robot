using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDashboard.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private AutomationDashboardDbContext dbContext;

        public AutomationDashboardDbContext Init()
        {
            return dbContext ?? (dbContext = new AutomationDashboardDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

    }
}
